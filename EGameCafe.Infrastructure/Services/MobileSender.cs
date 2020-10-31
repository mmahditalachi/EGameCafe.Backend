using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Application.Models.SMSIR;
using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Services
{
    public class MobileSender : IMobileSenders
    {
        private readonly Random _random = new Random();
        private readonly HttpClient _httpClient;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger<MobileSender> _logger;
        private readonly IMemoryCache _cache;

        public MobileSender(HttpClient httpClient, ILogger<MobileSender> logger, IApplicationDbContext applicationDbContext, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _cache = memoryCache;
        }

        public async Task<Result> SendOTP(string phoneNumber, string email, string token)
        {
            int otpNumber = GenerateRandomNumber();

            var optAsJson = GenerateJsonOPT(phoneNumber, otpNumber);

            await SaveOTP(token, email, otpNumber);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/VerificationCode");

            bool requestTimeout = CachingSenderTimeout(phoneNumber);

            if(!requestTimeout)
            {
                return Result.Failure("Server time out", "کد تایید برای شما ارسال شده است ، لطفا 30 ثانیه دیگر مجدد تلاش کنید");
            }

            var SMSToken = await GetToken();

            requestMessage.Headers.Add("x-sms-ir-secure-token", SMSToken);

            requestMessage.Content = new StringContent(optAsJson);

            requestMessage.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var result = JsonSerializer.Deserialize<SMSVerificationCode>(await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!result.IsSuccessful)
            {
                _logger.Log(LogLevel.Warning, $"SMS not sent to {phoneNumber} errorr : {result.Message}");
                throw new SMSException("Get Token", result.Message);
            }

            return Result.Success();
        }

        private async Task<string> GetToken()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Token");

            var serializedRequest = JsonSerializer.Serialize(new SendToken());

            requestMessage.Content = new StringContent(serializedRequest);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<SendTokenResult>(responseBody);

            return result.IsSuccessful 
                ? result.TokenKey
                : throw new SMSException("Get Token", result.Message);
        }

        private string GenerateJsonOPT(string phoneNumber,int otpNumber)
        {   
            var verificationCode = new VerificationCode()
            {
                MobileNumber = phoneNumber,
                Code = otpNumber.ToString()
            };

            return JsonSerializer.Serialize(verificationCode);
        }

        private async Task SaveOTP(string token, string userId, int optNumber)
        {
            var otp = new OTP()
            {
                RandomNumber = optNumber,
                Token = token,
                UserId = userId,
            };

            _applicationDbContext.OTP.Add(otp);

            await _applicationDbContext.SaveChangesAsync();

        }

        private bool CachingSenderTimeout(string username)
        {
            string cacheKey = username + "confiramtionCode";

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));

            if (_cache.TryGetValue(cacheKey, out int cacheData))
            {
                int timeout = cacheData + 1;

                _logger.Log(LogLevel.Warning, $"user {username} send confirmation {cacheData} times");

                if(cacheData > 4 )
                {
                    cacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromHours(1));

                    _cache.Set(cacheKey, timeout, cacheEntryOptions);

                    throw new RequestTimeoutException(username, "لطفا در یک ساعت آینده مجدد تلاش نمایید");
                }

                _cache.Set(cacheKey, timeout, cacheEntryOptions);

                return false;
            }

            _cache.Set(cacheKey, 1, cacheEntryOptions);

            return true;
        }

        private int GenerateRandomNumber()
        {
            return _random.Next(10000, 99999);
        }

    }
}
