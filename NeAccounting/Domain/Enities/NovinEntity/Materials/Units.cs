﻿using Domain.Common;
using DomainShared.Errore;

namespace Domain.NovinEntity.Materials
{
    public class Units : LocalEntity<Guid>
    {
        #region Navigation
        public ICollection<Pun> Materials { get; private set; }
        #endregion

        #region Property
        public string Name { get; private set; }
        public string? Descrip { get;private set; }
        public bool IsActive { get; set; }
        #endregion

        #region ctor
        internal Units()
        {

        }

        public Units(string name,
            string description)
        {
            SetName(name);
            SetDesc(description);
            IsActive = true;
        }
        public Units(
            string name,
            string description,
            Guid id) : this(name, description)
        {
            Id = id;
        }
        #endregion

        #region Methods
        public Units SetName(string name)
        {
            if (name.Length > 30)
                throw new ArgumentException(NeErrorCodes.IsLess("نام واحد", "سی"));

            Name = name;
            return this;
        }

        public Units SetDesc(string? description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 100)
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "صد"));

            Descrip = description;
            return this;
        }
        #endregion
    }
}