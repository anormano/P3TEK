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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class JournalVoucher : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public JournalVoucher(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [Action(AutoCommit = true, Caption = "Post", ConfirmationMessage = "Posted journal can't be modified and must be corrected by reverse journal./r/nAre your sure want to continue?")]
        public void PostJournal()
        {

        }

        string voucherNumber;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [Key]
        [RuleRequiredField]
        [RuleUniqueValue]
        public string VoucherNumber
        {
            get => voucherNumber;
            set => SetPropertyValue(nameof(VoucherNumber), ref voucherNumber, value);
        }

        int financialYear;
        public int FinancialYear
        {
            get => financialYear;
            set => SetPropertyValue(nameof(FinancialYear), ref financialYear, value);
        }

        bool posted;
        [CaptionsForBoolValues("Posted", "Not Posted")]
        public bool Posted
        {
            get => posted;
            set => SetPropertyValue(nameof(Posted), ref posted, value);
        }

        DateTime transactionDate;
        public DateTime TransactionDate
        {
            get => transactionDate;
            set => SetPropertyValue(nameof(TransactionDate), ref transactionDate, value);
        }

        string description;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [PersistentAlias("Details[Amount > 0].Sum(Amount)")]
        public decimal Debit
        {
            get
            {
                try
                {
                    return (decimal)EvaluateAlias(nameof(Debit));
                }

                catch
                {
                    return 0;
                }
            }
        }

        [PersistentAlias("Voucher[Amount < 0].Sum(Amount)")]
        public decimal Credit
        {
            get
            {
                try
                {
                    return (decimal)EvaluateAlias(nameof(Credit));
                }

                catch
                {
                    return 0;
                }
            }
        }

        [Association("JournalVoucher-Details")]
        public XPCollection<VoucherDetail> Details
        {
            get
            {
                return GetCollection<VoucherDetail>(nameof(Details));
            }
        }
    }
}