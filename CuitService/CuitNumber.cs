using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CuitService
{
    // TODO: move validation inside constructor and ModelState updates inside a modelbinder,
    // see https://docs.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-3.1#custom-model-binder-sample
    // TODO: implement IEQualable and IComparable
    // see https://andrewlock.net/using-strongly-typed-entity-ids-to-avoid-primitive-obsession-part-2/
    [JsonConverter(typeof(CuitNumberJsonConverter))]
    [TypeConverter(typeof(CuitNumberTypeConverter))]
    // TODO: change it to `readonly struct`, by the moment it does not work because it does not work well with IValidatableObject
    public class CuitNumber : IValidatableObject
    {
        class CuitNumberJsonConverter : JsonConverter<CuitNumber>
        {
            public override CuitNumber Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
            {
                return new CuitNumber(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, CuitNumber value, JsonSerializerOptions options)
            {
                // TODO: usar el formato bonito con guiones (PrettyValue o FormattedValue)
                writer.WriteStringValue(value.OriginalValue);
            }
        }

        class CuitNumberTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var stringValue = value as string;

                if (string.IsNullOrEmpty(stringValue))
                {
                    return base.ConvertFrom(context, culture, value);
                }

                return new CuitNumber(stringValue);
            }
        }

        public string? OriginalValue { get; }
        public string SimplifiedValue => OriginalValue?.Replace("-", "") ?? string.Empty;
        // TODO: add a new field Formatted Value, and return that value in ToString

        public CuitNumber(string? value)
        {
            // TODO: validate value and throw exceptions on wrong values
            OriginalValue = value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OriginalValue == null)
            {
                return new ValidationResult[0];
            }

            if (string.IsNullOrWhiteSpace(SimplifiedValue))
            {
                return new[] { new ValidationResult("The CUIT number cannot be empty.") };
            }

            if (!SimplifiedValue.All(char.IsNumber))
            {
                return new[] { new ValidationResult("The CUIT number cannot have other characters than numbers and dashes.") };
            }

            if (SimplifiedValue.Length != 11)
            {
                return new[] { new ValidationResult("The CUIT number cannot have less than 11 numbers.") };
            }

            if (!IsVerificationDigitValid(SimplifiedValue))
            {
                return new[] { new ValidationResult("The CUIT's verification digit is wrong.") };
            }

            return new ValidationResult[0];
        }


        // Source: https://es.wikipedia.org/wiki/Clave_%C3%9Anica_de_Identificaci%C3%B3n_Tributaria
        private static bool IsVerificationDigitValid(string normalizedCuit)
        {
            var factors = new int[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

            var accumulated = 0;

            for (int i = 0; i < factors.Length; i++)
            {
                accumulated += int.Parse(normalizedCuit[i].ToString()) * factors[i];
            }

            accumulated = 11 - (accumulated % 11);

            if (accumulated == 11)
            {
                accumulated = 0;
            }

            if (int.Parse(normalizedCuit[10].ToString()) != accumulated)
            {
                return false;
            }

            return true;
        }
    }
}
