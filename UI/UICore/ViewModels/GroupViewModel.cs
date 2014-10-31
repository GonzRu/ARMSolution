using CoreLib.ExchangeProviders;
using CoreLib.Models.Configuration;
using System.Collections.Generic;
using System.ComponentModel;

namespace UICore.ViewModels
{
    public class GroupViewModel : ViewModelBase
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
        public List<GroupViewModel> SubGroups { get; set; }

        /// <summary>
        /// Теги
        /// </summary>
        [Browsable(false)]
        public List<TagViewModel> Tags { get; set; }

        #endregion

        #region Private fields

        /// <summary>
        /// Ссылка на модель группы
        /// </summary>
        protected readonly Group Group;

        protected IExchangeProvider ExchangeProvider;

        #endregion

        #region Constructor

        public GroupViewModel(Group group, IExchangeProvider exchangeProvider)
        {
            Group = group;
            ExchangeProvider = exchangeProvider;

            SubGroups = new List<GroupViewModel>();
            foreach (var subgroup in group.SubGroups)
                SubGroups.Add(new GroupViewModel(subgroup, exchangeProvider));

            if (Group.Tags.Count != 0)
            {
                Tags = new List<TagViewModel>();
                foreach (var tag in Group.Tags)
                    if (tag is TagAnalog)
                        Tags.Add(new AnalogTagViewModel(tag as TagAnalog, exchangeProvider));
                    else if (tag is TagDiscret)
                        Tags.Add(new BaseTagDiscretViewModel(tag as TagDiscret, exchangeProvider));
                    else
                        Tags.Add(new TagViewModel(tag, exchangeProvider));
            }
        }

        #endregion
    }
}
