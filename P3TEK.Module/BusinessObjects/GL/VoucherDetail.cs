using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace P3TEK.Module.BusinessObjects.GL
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    [CreatableItem(false)]
    [DefaultListViewOptions(true, NewItemRowPosition.Top)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class VoucherDetail : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VoucherDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        int detailID;
        [RuleRequiredField]
        [RuleUniqueValue]
        [Key(true)]
        public int DetailID
        {
            get => detailID;
            set => SetPropertyValue(nameof(DetailID), ref detailID, value);
        }

        JournalVoucher voucher; 
        [Association("JournalVoucher-Details")]
        public JournalVoucher Voucher
        {
            get => voucher;
            set => SetPropertyValue(nameof(Voucher), ref voucher, value);
        }

        GLAccount account;
        [DataSourceCriteria("[Suspended] = False")]
        [RuleRequiredField]
        public GLAccount Account
        {
            get => account;
            set => SetPropertyValue(nameof(Account), ref account, value);
        }

        decimal amount;
        [ModelDefault("Caption", "Debit/(Credit)")]
        [RuleValueComparison(ValueComparisonType.NotEquals, 0)]
        public decimal Amount
        {
            get => amount;
            set => SetPropertyValue(nameof(Amount), ref amount, value);
        }

        string description;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }
    }
}