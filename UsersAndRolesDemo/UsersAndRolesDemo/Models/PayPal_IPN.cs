using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Web;

namespace UsersAndRolesDemo.Models
{
    public class PayPal_IPN
    {
        string _txnID, _txnType, _paymentStatus, _receiverEmail, _itemName, _itemNumber, _quantity, _invoice, _custom,
        _paymentGross, _payerEmail, _pendingReason, _paymentDate, _paymentFee, _firstName, _lastName, _address,
        _city, _state, _zip, _country, _countryCode, _addressStatus, _payerStatus, _payerID, _paymentType, _notifyVersion,
        _verifySign, _response, _payerPhone, _payerBusinessName, _business, _receiverID, _memo, _tax, _qtyCartItems,
        _shippingMethod, _shipping;
        private string _postUrl = "";
        private string _strRequest = "";
        private string _smtpHost, _fromEmail, _toEmail, _fromEmailPassword, _smtpPort;
        /// <summary>
        /// valid strings are "TEST" for sandbox use 
        /// "LIVE" for production use
        /// "ELITE" for test use off of PayPal...avoid having to be logged into PayPal Developer
        /// </summary>
        /// <param name="mode"></param>
        public PayPal_IPN(string mode)
        {
            if (mode.ToLower() == "test")
                this.PostUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            else if (mode.ToLower() == "live")
                this.PostUrl = "https://www.paypal.com/cgi-bin/webscr";
            else if (mode.ToLower() == "elite")
                this.PostUrl = "http://www.eliteweaver.co.uk/testing/ipntest.php";
            else
                this.PostUrl = "";
            this.fillProperties();
        }
        public string PostUrl
        {
            get { return _postUrl; }
            set { _postUrl = value; }
        }
        /// <summary>
        /// This is the reponse back from the http post back to PayPal.
        /// Possible values are "VERIFIED" or "INVALID"
        /// </summary>
        public string Response
        {
            get { return _response; }
        }
        public string RequestLength
        {
            get { return _strRequest; }
        }
        public string SmtpHost
        {
            get { return _smtpHost; }
        }
        public string SmtpPort
        {
            get { return _smtpPort; }
        }
        public string FromEmail
        {
            get { return _fromEmail; }
        }
        public string FromEmailPassword
        {
            get { return _fromEmailPassword; }
        }
        public string ToEmail
        {
            get { return _toEmail; }
        }
        /// <summary>
        /// Email address or Account ID of the payment recipient.  This is equivalent
        ///  to the value of receiver_email if the payment is sent to the primary account
        /// , which is most cases it is.  This value is that value of what is set in the button html
        /// markup.  This value also get normalized to lowercase when coming back from PayPal
        /// </summary>
        public string Business
        {
            get { return _business; }
            set { _business = value; }
        }
        /// <summary>
        /// Unique Paypal transaction ID
        /// </summary>
        public string TXN_ID { get { return _txnID; } }
        /// <summary>
        /// Type of transaction from the customer. Possible values are
        /// "cart", "express_checkout", "send_money", "virtual_terminal", "web-accept"
        /// </summary>
        public string TXN_Type { get { return _txnType; } }
        /// <summary>
        /// This is the status of the payment from the Customer.Possible values are: 
        /// "Canceled_Reversal", "Completed", "Denied", "Expired", "Failed", "Pending",
        ///  "Processed", "Refunded", "Reversed", "Voided"
        /// </summary>
        public string PaymentStatus { get { return _paymentStatus; } }
        public string ReceiverEmail { get { return _receiverEmail; } }
        public string ReceiverID { get { return _receiverID; } }
        public string ItemName { get { return _itemName; } }
        public string ItemNumber { get { return _itemNumber; } }
        public string Quantity { get { return _quantity; } }
        public string QuantityCartItems { get { return _qtyCartItems; } }
        public string Invoice { get { return _invoice; } }
        public string Custom { get { return _custom; } }
        public string Memo { get { return _memo; } }
        public string Tax { get { return _tax; } }
        public string PaymentGross { get { return _paymentGross; } }
        public string PaymentDate { get { return _paymentDate; } }
        public string PaymentFee { get { return _paymentFee; } }
        public string PayerEmail { get { return _payerEmail; } }
        public string PayerPhone { get { return _payerPhone; } }
        public string PayerBusinessName { get { return _payerBusinessName; } }
        public string PendingReason { get { return _pendingReason; } }
        public string ShippingMethod { get { return _shippingMethod; } }
        public string Shipping { get { return _shipping; } }
        public string PayerFirstName { get { return _firstName; } }
        public string PayerLastName { get { return _lastName; } }
        public string PayerAddress { get { return _address; } }
        public string PayerCity { get { return _city; } }
        public string PayerState { get { return _state; } }
        public string PayerZipCode { get { return _zip; } }
        public string PayerCountry { get { return _country; } }
        public string PayerCountryCode { get { return _countryCode; } }
        public string PayerAddressStatus { get { return _addressStatus; } }
        /// <summary>
        /// Customer either had a verified or unverified account with PayPal. 
        /// Possible return values from PayPal are "verified" or "unverified"
        /// </summary>
        public string PayerStatus { get { return _payerStatus; } }
        public string PayerID { get { return _payerID; } }
        public string PaymentType { get { return _paymentType; } }
        /// <summary>
        /// This is the version number of the IPN that makes the post.
        /// </summary>
        public string NotifyVersion { get { return _notifyVersion; } }
        /// <summary>
        /// An encrypted string that is used to validate the transaction. You don't have to use this for anything
        ///  unless you want to keep it and store it for your records.
        /// </summary>
        public string VerifySign { get { return _verifySign; } }
        private void fillProperties()
        {
            this._strRequest = HttpContext.Current.Request.Form.ToString();
            this._city = HttpContext.Current.Request.Form["address_city"];
            this._country = HttpContext.Current.Request.Form["address_country"];
            this._countryCode = HttpContext.Current.Request.Form["address_country_code"];
            this._state = HttpContext.Current.Request.Form["address_state"];
            this._addressStatus = HttpContext.Current.Request.Form["address_status"];
            this._address = HttpContext.Current.Request.Form["address_street"];
            this._zip = HttpContext.Current.Request.Form["address_zip"];
            this._firstName = HttpContext.Current.Request.Form["first_name"];
            this._lastName = HttpContext.Current.Request.Form["last_name"];
            this._payerBusinessName = HttpContext.Current.Request.Form["payer_business_name"];
            this._payerEmail = HttpContext.Current.Request.Form["payer_email"];
            this._payerID = HttpContext.Current.Request.Form["payer_id"];
            this._payerStatus = HttpContext.Current.Request.Form["payer_status"];
            this._payerPhone = HttpContext.Current.Request.Form["contact_phone"];
            this._business = HttpContext.Current.Request.Form["business"];
            this._itemName = HttpContext.Current.Request.Form["item_name"];
            this._itemNumber = HttpContext.Current.Request.Form["item_number"];
            this._quantity = HttpContext.Current.Request.Form["quantity"];
            this._receiverEmail = HttpContext.Current.Request.Form["receiver_email"];
            this._receiverID = HttpContext.Current.Request.Form["receiver_id"];
            this._custom = HttpContext.Current.Request.Form["custom"];
            this._memo = HttpContext.Current.Request.Form["memo"];
            this._invoice = HttpContext.Current.Request.Form["invoice"];
            this._tax = HttpContext.Current.Request.Form["tax"];
            this._qtyCartItems = HttpContext.Current.Request.Form["num_cart_items"];
            this._paymentDate = HttpContext.Current.Request.Form["payment_date"];
            this._paymentStatus = HttpContext.Current.Request.Form["payment_status"];
            this._paymentType = HttpContext.Current.Request.Form["payment_type"];
            this._pendingReason = HttpContext.Current.Request.Form["pending_reason"];
            this._txnID = HttpContext.Current.Request.Form["txn_id"];
            this._txnType = HttpContext.Current.Request.Form["txn_type"];
            this._paymentFee = HttpContext.Current.Request.Form["mc_fee"];
            this._paymentGross = HttpContext.Current.Request.Form["mc_gross"];
            this._notifyVersion = HttpContext.Current.Request.Form["notify_version"];
            this._verifySign = HttpContext.Current.Request.Form["verify_sign"];
        }
    }
}
