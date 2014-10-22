using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Windows.Data;

namespace SilverlightControlsLibrary
{
    

    #region Попытки перевести enum в строку
    
    #region TypeConverter для Enum, преобразовывающий Enum к строке с учетом атрибута Description // в сильверлайте System.ComponentModel не содержит EnumConverter
    /// <summary>
    /// TypeConverter для Enum, преобразовывающий Enum к строке с учетом атрибута Description
    /// </summary>
    //class EnumTypeConverter : EnumConverter
    //{
    //    private Type _enumType;
    //    /// <summary>Инициализирует экземпляр</summary>
    //    /// <param name="type">тип Enum</param>
    //    public EnumTypeConverter(Type type)
    //        : base(type)
    //    {
    //        _enumType = type;
    //    }

    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
    //    {
    //        return destType == typeof(string);
    //    }

    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
    //    {
    //        FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
    //        DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

    //        if (dna != null)
    //            return dna.Description;
    //        else
    //            return value.ToString();
    //    }

    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
    //    {
    //        return srcType == typeof(string);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        foreach (FieldInfo fi in _enumType.GetFields())
    //        {
    //            DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

    //            if ((dna != null) && ((string)value == dna.Description))
    //                return Enum.Parse(_enumType, fi.Name);
    //        }

    //        return Enum.Parse(_enumType, (string)value);
    //    }
    //}
    #endregion TypeConverter для Enum, преобразовывающий Enum к строке с учетом атрибута Description

    //===========================================================================================================================
    // Запихнуть enum в EnumContainer и через IValueConverter можно показать в комбобоксе, но в пропертигриде само по себе не появится (((
    //public class EnumContainer
    //{
    //    public int EnumValue { get; set; }
    //    public string EnumDescription { get; set; }
    //    public object EnumOriginalValue { get; set; }
    //    public override string ToString()
    //    {
    //        return EnumDescription;
    //    }
        
    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null)
    //            return false;
    //        return EnumValue.Equals((int)obj);
    //    }
        
    //    public override int GetHashCode()
    //    {
    //        return EnumValue.GetHashCode();
    //    }
    //}

    //public class EnumCollection<T> : List<EnumContainer> where T : struct
    //{
    //    public EnumCollection()
    //    {
    //        var type = typeof(T);
    //        if (!type.IsEnum)
    //            throw new ArgumentException("This class only supports Enum types");

    //        var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
    //        foreach (var field in fields)
    //        {
    //            var container = new EnumContainer();
    //            container.EnumOriginalValue = field.GetValue(null);
    //            container.EnumValue = (int)field.GetValue(null);
    //            container.EnumDescription = field.Name;
    //            var atts = field.GetCustomAttributes(false);
    //            foreach (var att in atts)
    //                if (att is DescriptionAttribute)
    //                {
    //                    container.EnumDescription = ((DescriptionAttribute)att).Description;
    //                    break;
    //                }
    //            Add(container);
    //        }
    //    }
    //}

    //public class EnumValueConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return (int)value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {

    //        if (value == null)
    //            return null;

    //        if (value.GetType() == targetType)
    //            return value;

    //        return ((EnumContainer)value).EnumOriginalValue;
    //    }
    //}
    //===========================================================================================================================

   

    //// Enum.Parse не работает, EnumConverter не видно в System.ComponentModel из Silverlight
    ///// <summary>
    ///// TypeConverter для Enum, преобразовывающий Enum к строке с учетом атрибута Description
    ///// </summary>
    //class MyEnumTypeConverter : TypeConverter// Вообще нужно наследовать от EnumConverter
    //{
    //    private Type _enumType;
    //    public MyEnumTypeConverter(Type type) : base()
    //    {
    //        _enumType = type;
    //    }

    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
    //    {
    //        return destType == typeof(string);
    //    }

    //    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
    //    {
    //        FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
    //        DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

    //        if (dna != null)
    //            return dna.Description;
    //        else
    //            return value.ToString();
    //    }

    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
    //    {
    //        return srcType == typeof(string);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    //    {
    //        foreach (FieldInfo fi in _enumType.GetFields())
    //        {
    //            DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

    //            if ((dna != null) && ((string)value == dna.Description))
    //                return Enum.Parse(_enumType, (string)fi.Name, true);
    //            //return Enum.Parse(typeof(ASUCommutationDeviceStates), value.ToString());
    //        }
    //        return Enum.Parse(_enumType, (string)value, true);
    //    }
    //}
    //============================================================================================
    //============================================================================================
    
    //// Так работает, строки отображаются в комбобоксе в пропертигриде, но сам enum недоступен из xaml (ни в виде "On", "Off" ни "Вкл.", "Откл.")
    //public class EnumToStringUsingDescription : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return (sourceType.Equals(typeof(Enum)));
    //    }

    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //    {
    //        return (destinationType.Equals(typeof(String)));
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    //    {
    //        return base.ConvertFrom(context, culture, value);
    //    }

    //    //public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    //    //{
    //    //    //return base.ConvertFrom(context, culture, value);

    //    //    if (!(value.GetType() == typeof(String)))
    //    //    {
    //    //        throw new ArgumentException("Конвертировать можно только из строки.", "value");
    //    //    }

    //    //    FieldInfo[] fis = typeof(ASUCommutationDeviceStates).GetFields();
    //    //    foreach (FieldInfo fi in fis)
    //    //    {
    //    //        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes (typeof(DescriptionAttribute), false);
    //    //        if (attributes.Length > 0)
    //    //        {
    //    //            if (attributes[0].Description.Equals((string)value))
    //    //            {
    //    //                return fi.Name;
    //    //            }
    //    //        }
    //    //    }
    //    //    return null;

    //    //}
    //    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (!destinationType.Equals(typeof(String)))
    //        {
    //            throw new ArgumentException("Конвертировать можно только в строку.", "destinationType");
    //        }

    //        if (!value.GetType().BaseType.Equals(typeof(Enum)))
    //        {
    //            throw new ArgumentException("Конвертировать можно только перечисление (enum).", "value");
    //        }

    //        string name = value.ToString();
    //        object[] attrs = value.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
    //        return (attrs.Length > 0) ? ((DescriptionAttribute)attrs[0]).Description : name;
    //    }
    //}

    //============================================================================================
    //============================================================================================
    // Так работает только если для каждого объекта специально вызывать MyDescription() 
    //public static class EnumExtensions
    //{
    //    public static string MyDescription(this Enum value)
    //    {
    //        var enumType = value.GetType();
    //        var field = enumType.GetField(value.ToString());
    //        var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
    //        return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
    //    }
    //}
    //============================================================================================
    //============================================================================================
   
    
    // Так работает, строки отображаются в комбобоксе в пропертигриде, но сам enum (On, Off, и т.д. недоступен из xaml)
    // [TypeConverter(typeof(MyEnumTypeConverter))]
    //public enum ASUCommutationDeviceStates
    //{
    //    #region Состояния КА
    //    [Description("Неопределенное")]
    //    UnDefined,
    //    [Description("Включено")]
    //    On,
    //    [Description("Отключено")]
    //    Off,
    //    [Description("Неисправность")]
    //    Broken,
    //    [Description("Выведен из работы")]
    //    OutOfWork,
    //    [Description("Включается")]
    //    TurningOn,
    //    [Description("Отключается")]
    //    TurningOff,
    //    [Description("Ручное заземление")]
    //    ManualPE
    //    #endregion Состояния КА
    //}
    #endregion Попытки перевести enum в строку

    public enum ASUSystemWorkStates
    {
        #region Режимы системы
        UnDefined,
        Duty,
        PE,
        FindPE,
        Control,
        LocalWithBlock,
        LocalWithoutBlock
        #endregion Режимы системы
    }

    //public enum ASUCommutationDeviceStateModes
    //{
    //    #region Способы формирования состояния КА
    //    TwoTagsModePRO_RPV,
    //    OneTagModeRPV,
    //    OneTagModeComputedState
        
    //    #endregion Способы формирования состояния КА
    //}

    public enum ASUCommutationDeviceStates
    {
        #region Состояния КА
        UnDefined,
        On,
        Off,
        Broken,
        OutOfWork,
        TurningOn,
        TurningOff,
        ManualPE
        #endregion Состояния КА
    }
    public enum ASUCellCartStates
    {
        #region Состояния тележки
        OutOfWork, 
        On,
        Off        
        #endregion Состояния тележки
    }
    public enum ASUCommutationDeviceControlModes
    {
        #region Режимы управления КА
        UnDefined,
        Distance,
        LocalWithBlocks,
        LocalWithoutBlocks
        #endregion Режимы управления КА
    }

    public enum ASUBlockStates
    {
        #region Состояния блокировок
        UnDefined,
        UnLocked,
        Locked
        #endregion Состояния блокировок
    }

    public enum ASUCommutationDeviceVoltageClasses
    {
        #region Классы напряжения
        [Description("Нет напряжения")]
        kVEmpty,
         [Description("1150 кВ")]
        kV1150,
         [Description("750 кВ")]
        kV750,
         [Description("500 кВ")]
        kV500,
         [Description("400 кВ")]
        kV400,
         [Description("330 кВ")]
        kV330,
         [Description("220 кВ")]
        kV220,
         [Description("150 кВ")]
        kV150,
         [Description("110 кВ")]
        kV110,
         [Description("35 кВ")]
        kV35,
         [Description("10 кВ")]
        kV10,
         [Description("6 кВ")]
        kV6,
         [Description("0,4 кВ")]
        kV04,
         [Description("Генерация")]
        kVGenerator,
         [Description("Ремонт")]
        kVRepair
        #endregion Классы напряжения
    }

    public enum ASUContentRelativelyPositions
    {
        #region Положение контента относительно объекта

        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Center
             
        #endregion Положение контента относительно объекта
    }
    public enum ASUCoilsConnectionTypes
    {
        #region Типы соединения обмоток трансформатора

        StarConnection,
        DeltaConnection,
        VConnection,
        NoCoils

        #endregion Типы соединения обмоток трансформатора
    }

    public enum ASUCoilExitPositions
    {
        #region Положение контента относительно объекта
        No,
        Left,
        Top,
        Right,
        Bottom
        #endregion Положение контента относительно объекта
    }

}
