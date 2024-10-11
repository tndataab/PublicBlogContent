// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityService;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };

            var largeAddress = new
            {
                street_address = "One Hacker Way-" + new string('x', 1500),
                locality = "Heidelberg-" + new string('y', 1500),
                postal_code = "6911 - " + new string('z', 1500),
                country = "Germany" + new string('A', 1500)
            };

            return new List<TestUser>
            {
                 new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "alice",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.PhoneNumber, "+46-708-6512224"),
                        new Claim(JwtClaimTypes.PhoneNumberVerified,"false"),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
                  
                                        
                        //custom claims
			            new Claim("employment_start","2020-01-02"),
                        new Claim("seniority","Admin"),
                        new Claim("contractor","yes"),
                        new Claim("employee","no"),
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.PhoneNumber, "+46-708-123456"),
                        new Claim(JwtClaimTypes.PhoneNumberVerified,"true"),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
              
                    
                        //custom claims
			            new Claim("employment_start","2019-01-02"),
                        new Claim("seniority","Senior"),
                        new Claim("contractor","no"),
                        new Claim("role","ceo"),
                        new Claim("role","finance"),
                        new Claim("role","developer"),
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "large",
                    Password = "large",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Large Larger"),
                        new Claim(JwtClaimTypes.GivenName, "Large"),
                        new Claim(JwtClaimTypes.FamilyName, "Medium"),
                        new Claim(JwtClaimTypes.Email, "large@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.PhoneNumber, "+46-708-123456"),
                        new Claim(JwtClaimTypes.PhoneNumberVerified,"true"),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(largeAddress), IdentityServerConstants.ClaimValueTypes.Json),
              
                    
                        //custom claims
			            new Claim("employment_start","2019-01-02"),
                        new Claim("seniority","Senior" + new string('x', 4500)),
                        new Claim("contractor","no"),
                        new Claim("role","ceo"),
                        new Claim("role","finance"),
                        new Claim("role","developer"),
                    }
                }
            };
        }
    }
}