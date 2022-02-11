﻿// <auto-generated />
using System;
using System.Reflection;
using DDocsBackend.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace DDocsBackend.Data.Cached
{
    internal partial class DiscordOAuthAuthenticationEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "DDocsBackend.Data.Models.DiscordOAuthAuthentication",
                typeof(DiscordOAuthAuthentication),
                baseEntityType);

            var userId = runtimeEntityType.AddProperty(
                "UserId",
                typeof(ulong),
                propertyInfo: typeof(DiscordOAuthAuthentication).GetProperty("UserId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(DiscordOAuthAuthentication).GetField("<UserId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                afterSaveBehavior: PropertySaveBehavior.Throw);

            var accessToken = runtimeEntityType.AddProperty(
                "AccessToken",
                typeof(string),
                propertyInfo: typeof(DiscordOAuthAuthentication).GetProperty("AccessToken", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(DiscordOAuthAuthentication).GetField("<AccessToken>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var expiresAt = runtimeEntityType.AddProperty(
                "ExpiresAt",
                typeof(DateTimeOffset),
                propertyInfo: typeof(DiscordOAuthAuthentication).GetProperty("ExpiresAt", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(DiscordOAuthAuthentication).GetField("<ExpiresAt>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var refreshToken = runtimeEntityType.AddProperty(
                "RefreshToken",
                typeof(string),
                propertyInfo: typeof(DiscordOAuthAuthentication).GetProperty("RefreshToken", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(DiscordOAuthAuthentication).GetField("<RefreshToken>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var key = runtimeEntityType.AddKey(
                new[] { userId });
            runtimeEntityType.SetPrimaryKey(key);

            return runtimeEntityType;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "DiscordAuthentication");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
