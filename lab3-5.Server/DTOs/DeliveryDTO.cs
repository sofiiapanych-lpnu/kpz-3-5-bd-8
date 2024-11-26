using lab3_5.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

public partial class DeliveryDTO
{
    public int DeliveryId { get; set; }

    public int? OrderId { get; set; }

    public int? CourierId { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    [DateAfter1900Attribute(ErrorMessage = "Date must be after the year 1900.")]
    public DateTime StartTime { get; set; }

    [JsonConverter(typeof(NullableDateTimeConverter))]
    [DateAfter1900Attribute(ErrorMessage = "Date must be after the year 1900.")]
    public DateTime? EndTime { get; set; }

    public TimeSpan? DesiredDuration { get; set; }

    [JsonConverter(typeof(NullableTimeSpanConverter))]
    public TimeSpan? ActualDuration { get; set; }

    public int? WarehouseId { get; set; }

    public int? AddressId { get; set; }

    public string? Status { get; set; }

    public virtual DeliveryAddress? Address { get; set; }

    public virtual Courier? Courier { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Warehouse? Warehouse { get; set; }

    public class DateAfter1900Attribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Year <= 1900)
                {
                    return new ValidationResult(ErrorMessage ?? "Date must be after the year 1900.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null ||
            (reader.TokenType == JsonTokenType.String && string.IsNullOrWhiteSpace(reader.GetString())))
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String && DateTime.TryParse(reader.GetString(), out var result))
        {
            return result;
        }

        throw new JsonException("Invalid date format.");
    }



    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString("o"));
        }
    }
}

public class NullableTimeSpanConverter : JsonConverter<TimeSpan?>
{
    public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && string.IsNullOrWhiteSpace(reader.GetString()))
        {
            return null;
        }
        return TimeSpan.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString());
        }
    }
}
