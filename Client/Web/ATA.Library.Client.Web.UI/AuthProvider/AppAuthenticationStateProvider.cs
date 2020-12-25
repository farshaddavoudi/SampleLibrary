using ATA.Library.Client.Service.SecurityClient.Contract;
using ATA.Library.Client.Web.Service.AppSetting;
using ATA.Library.Client.Web.UI.Extensions;
using ATA.Library.Shared.Core;
using ATA.Library.Shared.Service.Exceptions;
using ATA.Library.Shared.Service.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.AuthProvider
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _hostClient;
        private readonly ISecurityClientService _securityClientService;
        private readonly NavigationManager _navigationManager;
        private readonly AppSettings _appSettings;
        private readonly IWebAssemblyHostEnvironment _hostEnvironment;
        private readonly string _authTokenKey = AppStrings.ATAAuthTokenKey;

        #region Constructor Injections

        public AppAuthenticationStateProvider(HttpClient hostClient,
            NavigationManager navigationManager,
            AppSettings appSettings,
            IJSRuntime jsRuntime,
            IWebAssemblyHostEnvironment hostEnvironment,
            ISecurityClientService securityClientService)
        {
            _hostClient = hostClient;
            _navigationManager = navigationManager;
            _appSettings = appSettings;
            _jsRuntime = jsRuntime;
            _hostEnvironment = hostEnvironment;
            _securityClientService = securityClientService;
        }

        #endregion

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.GetCookieAsync(_authTokenKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                await NavigateToLoginPageOnSecurityApp();
            }

            else
            {
                // Call Security client to confirm token
                var securityResponse = await _securityClientService.GetUserByTokenAsync(token, default);


                // No valid response or token has been expired
                if (securityResponse == null || !securityResponse.IsSuccessful)
                    await NavigateToLoginPageOnSecurityApp();

                else //Valid token
                {
                    // Add token to all host http calls
                    _hostClient.DefaultRequestHeaders.Add(_authTokenKey, token);
                    _hostClient.DefaultRequestHeaders.Add("client-name", AppStrings.WebApp.ClientName);

                    // Create claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, $"{securityResponse.Data!.FName} {securityResponse.Data!.LName}"),
                        new Claim(ClaimTypes.NameIdentifier, securityResponse.Data!.UserID.ToString()),
                        new Claim(AppStrings.Claims.PersonnelCode, securityResponse.Data!.InfperCode.ToString()),
                        new Claim(AppStrings.Claims.Username, securityResponse.Data!.Username!)
                    };

                    // Add Roles to claims
                    var userRoles = await _securityClientService.GetUserRolesAsync(token, CancellationToken.None);

                    if (userRoles != null)
                        foreach (var role in userRoles)
                        {
                            if (role.Tag == null)
                                throw new DomainLogicException($"Role should have tag. Details: {role.SerializeToJson()}");

                            if (role.IsAdmin)
                            {
                                if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == AppStrings.Claims.Administrator))
                                    claims.Add(new Claim(ClaimTypes.Role, AppStrings.Claims.Administrator));
                            }

                            if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role.Tag))
                                claims.Add(new Claim(ClaimTypes.Role, role.Tag));
                        }

                    // Creates ClaimsIdentity
                    var claimsIdentity = new ClaimsIdentity(claims, "ATASecurityAuthType");

                    // Creates ClaimsPrinciple
                    var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

                    return new AuthenticationState(claimsPrinciple);
                }
            }

            // Anonymous 
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>())));
        }

        private async Task NavigateToLoginPageOnSecurityApp()
        {
            if (_hostEnvironment.IsDevelopment())
            {
                var securityResponse =
                    await _securityClientService.GetUserTokenByPersonnelCodeAsync(980923, CancellationToken.None);

                // Set auth cookie
                await _jsRuntime.SetCookieAsync(_authTokenKey, securityResponse!.Data!.Token);
            }
            else
            {
                _navigationManager.NavigateTo($"{_appSettings.Urls!.SecurityAppUrl}application/login.aspx?ReturnUrl={_appSettings.Urls.AppAddress}");
            }
        }
    }
}