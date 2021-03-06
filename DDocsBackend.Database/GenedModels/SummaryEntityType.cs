// <auto-generated />
using System;
using System.Collections.Generic;
using System.Reflection;
using DDocsBackend.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace DDocsBackend.Data.Cached
{
    internal partial class SummaryEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "DDocsBackend.Data.Models.Summary",
                typeof(Summary),
                baseEntityType);

            var summaryId = runtimeEntityType.AddProperty(
                "SummaryId",
                typeof(Guid),
                propertyInfo: typeof(Summary).GetProperty("SummaryId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<SummaryId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw);

            var content = runtimeEntityType.AddProperty(
                "Content",
                typeof(string),
                propertyInfo: typeof(Summary).GetProperty("Content", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<Content>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var eventDraftDraftId = runtimeEntityType.AddProperty(
                "EventDraftDraftId",
                typeof(Guid?),
                nullable: true);

            var eventId = runtimeEntityType.AddProperty(
                "EventId",
                typeof(Guid?),
                nullable: true);

            var isNew = runtimeEntityType.AddProperty(
                "IsNew",
                typeof(bool),
                propertyInfo: typeof(Summary).GetProperty("IsNew", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<IsNew>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var lastRevised = runtimeEntityType.AddProperty(
                "LastRevised",
                typeof(DateTimeOffset),
                propertyInfo: typeof(Summary).GetProperty("LastRevised", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<LastRevised>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var status = runtimeEntityType.AddProperty(
                "Status",
                typeof(FeatureType?),
                propertyInfo: typeof(Summary).GetProperty("Status", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<Status>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var title = runtimeEntityType.AddProperty(
                "Title",
                typeof(string),
                propertyInfo: typeof(Summary).GetProperty("Title", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<Title>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var type = runtimeEntityType.AddProperty(
                "Type",
                typeof(SummaryType),
                propertyInfo: typeof(Summary).GetProperty("Type", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<Type>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            var url = runtimeEntityType.AddProperty(
                "Url",
                typeof(string),
                propertyInfo: typeof(Summary).GetProperty("Url", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Summary).GetField("<Url>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var key = runtimeEntityType.AddKey(
                new[] { summaryId });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { eventDraftDraftId });

            var index0 = runtimeEntityType.AddIndex(
                new[] { eventId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("EventDraftDraftId")! },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("DraftId")! })!,
                principalEntityType);

            var summaries = principalEntityType.AddNavigation("Summaries",
                runtimeForeignKey,
                onDependent: false,
                typeof(ICollection<Summary>),
                propertyInfo: typeof(EventDraft).GetProperty("Summaries", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(EventDraft).GetField("<Summaries>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static RuntimeForeignKey CreateForeignKey2(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("EventId")! },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("EventId")! })!,
                principalEntityType);

            var summaries = principalEntityType.AddNavigation("Summaries",
                runtimeForeignKey,
                onDependent: false,
                typeof(ICollection<Summary>),
                propertyInfo: typeof(Event).GetProperty("Summaries", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Event).GetField("<Summaries>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Summaries");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
