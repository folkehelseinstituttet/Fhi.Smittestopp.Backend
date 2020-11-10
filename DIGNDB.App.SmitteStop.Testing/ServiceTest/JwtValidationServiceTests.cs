using System;
using DIGNDB.App.SmitteStop.Core.DependencyInjection;
using DIGNDB.App.SmitteStop.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class JwtValidationServiceTests
    {
        private readonly IJwtValidationService _jwtValidationService;

        public JwtValidationServiceTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCoreDependencies();
            // Add mocks to serviceCollection here

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            _jwtValidationService = serviceProvider.GetService<IJwtValidationService>();
        }

        private const string ValidToken =
        "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjIifQ.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA1MTc2MjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEifQ.AMs2TIXqmqA6uOXQD7Oaa8cgiMcd9Ww-8Qk1xysE7lJem2IM2FNWg9f54KH5HaE2tCS1Pm9rgmGDbz9gG6V9wSE7-bVS4JUrRpqknCswdctYbjy33oILGfHcFCuDGPBAkKcocw9kidXzblWbsYoM_JvmfMBVAlPsFGA6hdcfdnsYduLV_LvYQVUm50y6kcttb8VdmOeCmXiT2o6BdqPcuteQ-WKa0U0KSuJ0zlAlfHOSMMuj237QTqRYdHrCu76ByWv1U1ak6lZsAyYdvhzUw9oGcOSnHoz0nDRRrSnfLYqi9jCyVTZm6acvNRkSjWGqyCIO5gRDuRu42oWQC_EgUQ";

        [Test]
        public void TestValidateToken_WithValidToken()
        {
            var validationResult = _jwtValidationService.IsTokenValid(ValidToken);

            validationResult.Should().BeTrue();
        }

        [Test]
        public void TestValidateToken_WithValidTokenButInReplayAttack()
        {

            var validationResult = _jwtValidationService.IsTokenValid(ValidToken);
            validationResult.Should().BeTrue();

            Action validateAction = () => _jwtValidationService.IsTokenValid(ValidToken);

            validateAction.Should().Throw<SecurityTokenReplayDetectedException>();
        }

        private const string TokenWithSignatureAlgorithmHS256 = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA1MTc2MjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.AOp-quffQUTHKfep59kykD2M5c6FPLrEhQ5RYiKeQu0";
        private const string TokenWithSignatureAlgorithmHS384 = "eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA1MTc2MjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.n06mlIRCwOmsa36TmRBk7tp6VCjFnthI-M4lH0TWR_fPOwdIc4vQlMDBhQloLVkY";
        private const string TokenWithSignatureAlgorithmHS512 = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA1MTc2MjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.bz8TbcRL5VlolvVbRW_5l6b5c4fsjtjtLpQgZqoGKylIKGJAqyp9Vb_7B4h6BreqY8VMqF7VfIGvkC8IcBWeEQ";
        private const string TokenWithSignatureAlgorithmPS256 = "eyJhbGciOiJQUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA1MTc2MjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.H8FUFtgq-TiaObdWchhJDbufU9LpwJIG5gxVn1U9vBhLeImCiLnkV2_TSh0ihuK0XihtBIouIXkbiRFWvz97hVIOJQrw8pwi_2ch5S3f8QAbfRnCd8-SrfQGqWGB06tVZNOcucg1iQhb8BQydkwJIhxyZjYSRGyTjf1NZ3RbJ_lYz0DL8TbyUyERllvZVx0KA7951H4X_lJX2VYUVz0aWzNvUb2JGutalEAKuclzEvxVsLJr_GWUPizcC2KvdEmK78e5b93TSd-jqTm_aYZX-7M2XbynUTQKCTCDz2R_riSYZa3V9YT4cs4GtPx1ScO3tNU_DT7UT0bmTbx6-KQpKA";
        private const string TokenWithSignatureAlgorithmPS384 = "eyJhbGciOiJQUzM4NCIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA1MTc2MjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.A030A2mbXJmDxz2vpuPobfCiYeGwLq5Mthxm9p7O5WsSgxxXh_lZtGjyMSfG06YacLXygHBA3p2CywAQxabkWjqhHY5zufWzy92rvMT0-cYWcagm5ocYX6R76hthO4B-BbOpuiM6lx42DHDe8dsRyJTHKXj1k7tGZMV9UVCbK4RA_Ek3lN5vYMn5nA4zPOc75GuE291FNmRIpejXOcs_p76OqzLITH_JwO8dDGyRGnEE1aOaYIoYSKQuxcJOPVxwGjuAi2cX9vLwlCN03kCShlZ0Czy6zgrN6DdP_cxxoUy_Q9bl7feVmyiJR2g9iaeoN0dz841f1AejEXKNk6fQrg";

        [TestCase(TokenWithSignatureAlgorithmHS256)]
        [TestCase(TokenWithSignatureAlgorithmHS384)]
        [TestCase(TokenWithSignatureAlgorithmHS512)]
        [TestCase(TokenWithSignatureAlgorithmPS256)]
        [TestCase(TokenWithSignatureAlgorithmPS384)]
        public void TestValidateToken_WithInvalidSignatureAlgorithm(string token)
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(token);

            validateAction.Should().Throw<SecurityTokenInvalidSignatureException>()
                .WithMessage("IDX10503: Signature validation failed.*");
        }

        private const string TokenWithInvalidAuthorizedParty =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA1MTc2MjI1LCJhenAiOiJ4eHgiLCJpc3MiOiJodHRwczovL2Rldi1zbWl0dGVzdG9wcC12ZXJpZmljYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQiLCJqdGkiOiIxIiwia2lkIjoiMiJ9.RvCk9emy-VH8ya65MRDSEq83neImGBGDcKcfayLtEQ0AbP-dtgO-2qHpC6E7BbOHf6rAkS3KGJT82lG1xCPCQtEcz7x8k1VkiOCTL-wQltEck2XDnkMh6kUyW_PseY2k8HZkbd5HPEyIP2NLVZSmjs4D3WMgAeJcEjBqaQASeX8dvUiQ5V1168FHtZzbbD12LWyY0xdZKuTEjgW7uD4X_C7nvA57YGTv5YajAtlx-0_pRQagKIUggaJZ0FgImGJ4TqMQy4Jua6gV1XErDTTF2RHepaVRdt2bIkPlkvdyzD7VahqbjyjtNnSdS1PZ4QDulgsjxkieDUTcXwZ5X1N6Tw";

        [Test]
        public void TestValidateToken_WithInvalidAuthorizedParty()
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(TokenWithInvalidAuthorizedParty);

            validateAction.Should().Throw<SecurityTokenException>()
                .WithMessage("AuthorizeParty claim is invalid.*");
        }

        private const string TokenWithExpiredTimeWindow =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDgzMDYyNSwiZXhwIjoxNjA0ODMwNjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.gvX2LbuOG9J-6b_71ON3xEN_vJaf9uffYNBvKSgT0eg3K1Z0q9Z4cm6nQtaWOldfxrLADC7AzAYQ14uc-PdaaOr-rRfihLxmXVc-iY6ktkTUZt6wSL8ds9lx6zOZIfNhoRbIVAVKFjxWfRkDVtesjA16kBRvA2Wjf8E05xGInKXDDXIond_XJPDdGY7NG6UUfQf5FoLdVgF1d4WryKBSAJlE6Cqm8Bx9hiWPQX86EWwhWMz4sq7Pvo1eOPrB92HZYnOKHLQFs1Vak8dj9OFfJd2pFLHuL029OLfiKHCQfUQDvJPKCSwd5IaeqXqOD4fHQi9WkbvW75vuS1ffj8Aksg";
        private const string TokenIssuedInTheFuture =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNTQzNTQyNSwiZXhwIjoxNjA0ODMwNjI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.QRyz4hov36Kd7SZX4KuWd7r6RnXbnAzRfXHndyT1frzzyHDxOx3KsQPaw9V7XgpxEQIMLHdzjlM05EjhjZfx1cWENHiCTws2yGoHioQ_CvL3NT_zQtpCYfgc-EieuLZBqVvl-KEo9ovn3lUlJdECD6DSZFHZjksESpaM_VA3fMNAekOozMMB3oIsrmQtVfdoBrRlSL-v1nKtXfe3ZrF4pw5LGoN98RFTZms-lpv6Yo3PcXADZypgeK9S8qXps49qP6oVRxgJTj12LcHebwFxJxmQq_iSg2TA8gtHXfG38mHGdGfESNIf91HzxCwlzDday-8mB_qNAMH-qy9GS8ggAw";

        [TestCase(TokenWithExpiredTimeWindow)]
        [TestCase(TokenIssuedInTheFuture)]
        public void TestValidateToken_WithInvalidTimeWindow(string token)
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(token);

            validateAction.Should().Throw<SecurityTokenExpiredException>()
                .WithMessage("IDX10223: Lifetime validation failed. The token is expired.*");
        }

        private const string TokenWithNotKnownPublicKey =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDU3MTQyNSwiZXhwIjoxNjA1ODY3NDI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8vZGV2LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiJ4In0.JHH7dxL9EPluLA7cPN6mSKKkORXplQHiONhEka7isRwM1qSRO-0a6UI84FzviWwhOI8e-voKF0MTT7YyEXfgDN_2P-VzC84GfDopfJDeHn1M2w6xLgi4wnm75lsM6M9El_VOo83L8sOCccsxH4qhFdRi6OqVjE-ycbIdA5gBNGqk0uQcqjXfZQQ896RGttKCzgEi6w03O9ydFhHu1hO0skBIm2-eqq2BOhUP9cqxs58HJELc7VExMs1GbPZjxvYoHdwXTvUDTSAMsqU08a4IIea-JzQKorDog2q2-78dcK1uaAoghneYJdfLN0NZEEX6Fa6ygROLOYLy6Lcobqfe5Q";

        [Test]
        public void TestValidateToken_WithUnknownPublicKey()
        {
            var validationResult = _jwtValidationService.IsTokenValid(TokenWithNotKnownPublicKey);

            validationResult.Should().BeFalse();
        }

        private const string TokenWithInvalidIssuer =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTYwNDU3MTQyNSwiZXhwIjoxNjA1ODY3NDI1LCJhenAiOiJzbWl0dGVzdG9wcCIsImlzcyI6Imh0dHBzOi8veHh4LXNtaXR0ZXN0b3BwLXZlcmlmaWNhdGlvbi5henVyZXdlYnNpdGVzLm5ldCIsImp0aSI6IjEiLCJraWQiOiIyIn0.SojLdcNsqCu5O4Maaf_Ns3TjpNX5BPwapCvViEqyJ96eYRsWhDGdzpKY9OPtLhj5iVJgMfeder9p-3ABZLv-QhjJ218C0TXAU9WEQCWFwA-mITCRKf6VgN6QszUVe-oAk-cTjEDDhUXqEj58oGVPpfvnqiJPn-gr_nYgIMRq5tNl2tpvJV69kcLzA1rarxsSwraOlfdwMuzVWOeqBGszVh5_OKzoeSi0ss1STF_AVAXpvAzEeN6kVNd__GkbQ-KIKJ46R3_kkLWaOAFqFQmXsDWgm5ONDM4SZR5BMPr9JOh81c2B9OlCFKrNlNZJ1C5vPUCPuM48YWiIeDYP3tmEoA";

        [Test]
        public void TestValidateToken_WithInvalidIssuer()
        {
            Action validateAction = () => _jwtValidationService.IsTokenValid(TokenWithInvalidIssuer);

            validateAction.Should().Throw<SecurityTokenInvalidIssuerException>()
                .WithMessage("IDX10205: Issuer validation failed.*");
        }
    }
}