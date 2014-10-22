using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System.Collections.Generic;
using System.ComponentModel;

namespace UICore.ViewModels
{
    public class BaseGroupViewModel : BaseViewModel
    {
        #region Public properties

        /// <summary>
        /// Имя группы
        /// </summary>
        [Category("Общее")]
        [DisplayName("Имя группы")]
        [Description("Имя группы")]
        public string GroupName
        {
            get { return Group.GroupName; }
            set
            {
                Group.GroupName = value;
                NotifyPropertyChanged("GroupName");
            }
        }

        [Category("Общее")]
        [DisplayName("Enable")]
        [Description("Показывать ли группу")]
        public bool Enable
        {
            get { return Group.Enable; }
            set { Group.Enable = value; }
        }

        [Category("Общее")]
        [DisplayName("Категория группы")]
        [Description("Категория группы")]
        public GroupCategory GroupCategory
        {
            get { return Group.GroupCategory; }
            set
            {
                Group.GroupCategory = value;
                NotifyPropertyChanged("GroupCategory");
            }
        }

        /// <summary>
        /// Подгруппы
        /// </summary>
        [Browsable(false)]
        public List<BaseGroupViewModel> SubGroups { get; set; }

        /// <summary>
        /// Теги
        /// </summary>
        [Browsable(false)]
        public List<BaseTagViewModel> Tags { get; set; }

        #endregion

        #region Private fields

        /// <summary>
        /// Ссылка на модель группы
        /// </summary>
        protected readonly Group Group;

        protected IExchangeProvider ExchangeProvider;

        #endregion

        #region Constructor

        public BaseGroupViewModel(Group group, IExchangeProvider exchangeProvider)
        {
            Group = group;
            ExchangeProvider = exchangeProvider;

            SubGroups = new List<BaseGroupViewModel>();
            foreach (var subgroup in group.SubGroups)
                SubGroups.Add(new BaseGroupViewModel(subgroup, exchangeProvider));

            if (Group.Tags.Count != 0)
            {
                Tags = new List<BaseTagViewModel>();
                foreach (var tag in Group.Tags)
                    //if (tag is TagAnalog)
                    //    Tags.Add(new BaseTagAnalogViewModel(tag as TagAnalog, exchangeProvider));
                    //else if (tag is TagDiscret)
                    //    Tags.Add(new BaseTagDiscretViewModel(tag as TagDiscret, exchangeProvider));
                    //else
                        Tags.Add(new BaseTagViewModel(tag, exchangeProvider));
            }
        }

        #endregion
    }
}
