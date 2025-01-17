﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace WSPagoServicio.WSVisa {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PaymentGWServicesSoap", Namespace="http://tempuri.org/")]
    public partial class PaymentGWServices : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback paymentgw_versionOperationCompleted;
        
        private System.Threading.SendOrPostCallback AuthorizationRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback authorizationRequestxmlOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PaymentGWServices() {
            this.Url = global::WSPagoServicio.Properties.Settings.Default.WSPagoServicio_TestVisa_PaymentGWServices;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event paymentgw_versionCompletedEventHandler paymentgw_versionCompleted;
        
        /// <remarks/>
        public event AuthorizationRequestCompletedEventHandler AuthorizationRequestCompleted;
        
        /// <remarks/>
        public event authorizationRequestxmlCompletedEventHandler authorizationRequestxmlCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/paymentgw_version", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string paymentgw_version() {
            object[] results = this.Invoke("paymentgw_version", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void paymentgw_versionAsync() {
            this.paymentgw_versionAsync(null);
        }
        
        /// <remarks/>
        public void paymentgw_versionAsync(object userState) {
            if ((this.paymentgw_versionOperationCompleted == null)) {
                this.paymentgw_versionOperationCompleted = new System.Threading.SendOrPostCallback(this.Onpaymentgw_versionOperationCompleted);
            }
            this.InvokeAsync("paymentgw_version", new object[0], this.paymentgw_versionOperationCompleted, userState);
        }
        
        private void Onpaymentgw_versionOperationCompleted(object arg) {
            if ((this.paymentgw_versionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.paymentgw_versionCompleted(this, new paymentgw_versionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AuthorizationResponse", Namespace="http://general_computing.com/paymentgw/types")]
        public AuthorizationResponse AuthorizationRequest([System.Xml.Serialization.XmlElementAttribute("AuthorizationRequest", Namespace="http://general_computing.com/paymentgw/types")] AuthorizationRequest AuthorizationRequest1) {
            object[] results = this.Invoke("AuthorizationRequest", new object[] {
                        AuthorizationRequest1});
            return ((AuthorizationResponse)(results[0]));
        }
        
        /// <remarks/>
        public void AuthorizationRequestAsync(AuthorizationRequest AuthorizationRequest1) {
            this.AuthorizationRequestAsync(AuthorizationRequest1, null);
        }
        
        /// <remarks/>
        public void AuthorizationRequestAsync(AuthorizationRequest AuthorizationRequest1, object userState) {
            if ((this.AuthorizationRequestOperationCompleted == null)) {
                this.AuthorizationRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAuthorizationRequestOperationCompleted);
            }
            this.InvokeAsync("AuthorizationRequest", new object[] {
                        AuthorizationRequest1}, this.AuthorizationRequestOperationCompleted, userState);
        }
        
        private void OnAuthorizationRequestOperationCompleted(object arg) {
            if ((this.AuthorizationRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AuthorizationRequestCompleted(this, new AuthorizationRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/authorizationRequestxml", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string authorizationRequestxml(string xml) {
            object[] results = this.Invoke("authorizationRequestxml", new object[] {
                        xml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void authorizationRequestxmlAsync(string xml) {
            this.authorizationRequestxmlAsync(xml, null);
        }
        
        /// <remarks/>
        public void authorizationRequestxmlAsync(string xml, object userState) {
            if ((this.authorizationRequestxmlOperationCompleted == null)) {
                this.authorizationRequestxmlOperationCompleted = new System.Threading.SendOrPostCallback(this.OnauthorizationRequestxmlOperationCompleted);
            }
            this.InvokeAsync("authorizationRequestxml", new object[] {
                        xml}, this.authorizationRequestxmlOperationCompleted, userState);
        }
        
        private void OnauthorizationRequestxmlOperationCompleted(object arg) {
            if ((this.authorizationRequestxmlCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.authorizationRequestxmlCompleted(this, new authorizationRequestxmlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://general_computing.com/paymentgw/types")]
    public partial class AuthorizationRequest {
        
        private Request authorizationRequest1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AuthorizationRequest")]
        public Request AuthorizationRequest1 {
            get {
                return this.authorizationRequest1Field;
            }
            set {
                this.authorizationRequest1Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://general_computing.com/paymentgw/types")]
    public partial class Request {
        
        private string posEntryModeField;
        
        private string panField;
        
        private string expdateField;
        
        private string amountField;
        
        private string track2DataField;
        
        private string cvv2Field;
        
        private string paymentgwIPField;
        
        private string shopperIPField;
        
        private string merchantServerIPField;
        
        private string merchantUserField;
        
        private string merchantPasswdField;
        
        private string terminalIdField;
        
        private string merchantField;
        
        private string messageTypeField;
        
        private string auditNumberField;
        
        private string additionalDataField;
        
        private Response responseField;
        
        /// <remarks/>
        public string posEntryMode {
            get {
                return this.posEntryModeField;
            }
            set {
                this.posEntryModeField = value;
            }
        }
        
        /// <remarks/>
        public string pan {
            get {
                return this.panField;
            }
            set {
                this.panField = value;
            }
        }
        
        /// <remarks/>
        public string expdate {
            get {
                return this.expdateField;
            }
            set {
                this.expdateField = value;
            }
        }
        
        /// <remarks/>
        public string amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public string track2Data {
            get {
                return this.track2DataField;
            }
            set {
                this.track2DataField = value;
            }
        }
        
        /// <remarks/>
        public string cvv2 {
            get {
                return this.cvv2Field;
            }
            set {
                this.cvv2Field = value;
            }
        }
        
        /// <remarks/>
        public string paymentgwIP {
            get {
                return this.paymentgwIPField;
            }
            set {
                this.paymentgwIPField = value;
            }
        }
        
        /// <remarks/>
        public string shopperIP {
            get {
                return this.shopperIPField;
            }
            set {
                this.shopperIPField = value;
            }
        }
        
        /// <remarks/>
        public string merchantServerIP {
            get {
                return this.merchantServerIPField;
            }
            set {
                this.merchantServerIPField = value;
            }
        }
        
        /// <remarks/>
        public string merchantUser {
            get {
                return this.merchantUserField;
            }
            set {
                this.merchantUserField = value;
            }
        }
        
        /// <remarks/>
        public string merchantPasswd {
            get {
                return this.merchantPasswdField;
            }
            set {
                this.merchantPasswdField = value;
            }
        }
        
        /// <remarks/>
        public string terminalId {
            get {
                return this.terminalIdField;
            }
            set {
                this.terminalIdField = value;
            }
        }
        
        /// <remarks/>
        public string merchant {
            get {
                return this.merchantField;
            }
            set {
                this.merchantField = value;
            }
        }
        
        /// <remarks/>
        public string messageType {
            get {
                return this.messageTypeField;
            }
            set {
                this.messageTypeField = value;
            }
        }
        
        /// <remarks/>
        public string auditNumber {
            get {
                return this.auditNumberField;
            }
            set {
                this.auditNumberField = value;
            }
        }
        
        /// <remarks/>
        public string additionalData {
            get {
                return this.additionalDataField;
            }
            set {
                this.additionalDataField = value;
            }
        }
        
        /// <remarks/>
        public Response response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://general_computing.com/paymentgw/types")]
    public partial class Response {
        
        private string auditNumberField;
        
        private string referenceNumberField;
        
        private string authorizationNumberField;
        
        private string responseCodeField;
        
        private string messageTypeField;
        
        private string signatureField;
        
        /// <remarks/>
        public string auditNumber {
            get {
                return this.auditNumberField;
            }
            set {
                this.auditNumberField = value;
            }
        }
        
        /// <remarks/>
        public string referenceNumber {
            get {
                return this.referenceNumberField;
            }
            set {
                this.referenceNumberField = value;
            }
        }
        
        /// <remarks/>
        public string authorizationNumber {
            get {
                return this.authorizationNumberField;
            }
            set {
                this.authorizationNumberField = value;
            }
        }
        
        /// <remarks/>
        public string responseCode {
            get {
                return this.responseCodeField;
            }
            set {
                this.responseCodeField = value;
            }
        }
        
        /// <remarks/>
        public string messageType {
            get {
                return this.messageTypeField;
            }
            set {
                this.messageTypeField = value;
            }
        }
        
        /// <remarks/>
        public string signature {
            get {
                return this.signatureField;
            }
            set {
                this.signatureField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://general_computing.com/paymentgw/types")]
    public partial class AuthorizationResponse {
        
        private Response responseField;
        
        /// <remarks/>
        public Response response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void paymentgw_versionCompletedEventHandler(object sender, paymentgw_versionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class paymentgw_versionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal paymentgw_versionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void AuthorizationRequestCompletedEventHandler(object sender, AuthorizationRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AuthorizationRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AuthorizationRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AuthorizationResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AuthorizationResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void authorizationRequestxmlCompletedEventHandler(object sender, authorizationRequestxmlCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class authorizationRequestxmlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal authorizationRequestxmlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591