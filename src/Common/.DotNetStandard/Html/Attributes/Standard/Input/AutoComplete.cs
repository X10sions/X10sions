using System.ComponentModel;

namespace Common.Html.Attributes {
  public interface IAutoCompleteHtmlAttribute : IHtmlAttribute<IAutoCompleteHtmlAttribute.Values > {
    public enum Values {
      [Description("Default. Autocomplete is on (enabled)")] on,
      [Description("Autocomplete is off (disabled)")] off,
      [Description("Expects the first line of the street address")] address_line1,
      [Description("Expects the second line of the street address")] address_line2,
      [Description("Expects the third line of the street address")] address_line3,
      [Description("Expects the first level of the address, e.g. the county")] address_level1,
      [Description("Expects the second level of the address, e.g. the city")] address_level2,
      [Description("Expects the third level of the address")] address_level3,
      [Description("Expects the fourth level of the address")] address_level4,
      [Description("Expects the full street address")] street_address,
      [Description("Expects the country code")] country,
      [Description("Expects the country name")] country_name,
      [Description("Expects the post code")] postal_code,
      [Description("Expects the full name")] name,
      [Description("Expects the middle name")] additional_name,
      [Description("Expects the last name")] family_name,
      [Description("Expects the first name")] given_name,
      [Description("Expects the title, like 'Mr', 'Ms' etc.")] honoric_prefix,
      [Description("Expects the suffix, like '5', 'Jr.' etc.")] honoric_suffix,
      [Description("Expects the nickname")] nickname,
      [Description("Expects the job title")] organization_title,
      [Description("Expects the username")] username,
      [Description("Expects a new password")] new_password,
      [Description("Expects the current password")] current_password,
      [Description("Expects the full birthday date")] bday,
      [Description("Expects the day of the birthday date")] bday_day,
      [Description("Expects the month of the birthday date")] bday_month,
      [Description("Expects the year of the birthday date")] bday_year,
      [Description("Expects the gender")] sex,
      [Description("Expects a one time code for verification etc.")] one_time_code,
      [Description("Expects the company name")] organization,
      [Description("Expects the credit card owner's full name")] cc_name,
      [Description("Expects the credit card owner's first name")] cc_given_name,
      [Description("Expects the credit card owner's middle name")] cc_additional_name,
      [Description("Expects the credit card owner's full name")] cc_family_name,
      [Description("Expects the credit card's number")] cc_number,
      [Description("Expects the credit card's expiration date")] cc_exp,
      [Description("Expects the credit card's expiration month")] cc_exp_month,
      [Description("Expects the credit card's expiration year")] cc_exp_year,
      [Description("Expects the CVC code")] cc_csc,
      [Description("Expects the credit card's type of payment")] cc_type,
      [Description("Expects the currency")] transaction_currency,
      [Description("Expects a number, the amount")] transaction_amount,
      [Description("Expects the preferred language")] language,
      [Description("Expects a we address")] url,
      [Description("Expects the email address")] email,
      [Description("Expects an image")] photo,
      [Description("Expects the full phone number")] tel,
      [Description("Expects the country code of the phone number")] tel_country_code,
      [Description("Expects the phone number with no country code")] tel_national,
      [Description("Expects the area code of the phone number")] tel_area_code,
      [Description("Expects the phone number with no country code and no area code")] tel_local,
      [Description("Expects the local prefix of the phone number")] tel_local_prefix,
      [Description("Expects the local suffix of the phone number")] tel_local_suffix,
      [Description("Expects the extension code of the phone number")] tel_extension,
      [Description("Expects the url of an instant messaging protocol endpoint")] impp
    }
  };

  public readonly record struct AutoCompleteHtmlAttribute(IAutoCompleteHtmlAttribute.Values  Value) : IAutoCompleteHtmlAttribute {
    public IHtmlAttribute.Key Name { get; } = IHtmlAttribute.Key.autocomplete;
  }
}

