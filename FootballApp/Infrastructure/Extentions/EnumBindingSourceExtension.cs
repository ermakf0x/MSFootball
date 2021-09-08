using System;
using System.Windows.Markup;

namespace FootballApp.Infrastructure.Extentions
{
    class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumeType { get; }

        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
                throw new ArgumentException();

            EnumeType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumeType);
        }
    }
}
