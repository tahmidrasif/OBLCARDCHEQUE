﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CardChequeModule.ServiceReference {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://onebank.com.bd/", ConfigurationName="ServiceReference.OBLAPPSoap")]
    public interface OBLAPPSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string HelloWorld();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetAllUser", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetAllUser();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetAllUser", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetAllUserAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetEmployeeNameBy", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetEmployeeNameBy(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetEmployeeNameBy", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetEmployeeNameByAsync(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetIDbyEmployeeId", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        long GetIDbyEmployeeId(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetIDbyEmployeeId", ReplyAction="*")]
        System.Threading.Tasks.Task<long> GetIDbyEmployeeIdAsync(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreDeptNameBy", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetPreDeptNameBy(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreDeptNameBy", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetPreDeptNameByAsync(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreDeptIdBy", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int GetPreDeptIdBy(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreDeptIdBy", ReplyAction="*")]
        System.Threading.Tasks.Task<int> GetPreDeptIdByAsync(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreBranchIdBy", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int GetPreBranchIdBy(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreBranchIdBy", ReplyAction="*")]
        System.Threading.Tasks.Task<int> GetPreBranchIdByAsync(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreDesigBy", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int GetPreDesigBy(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetPreDesigBy", ReplyAction="*")]
        System.Threading.Tasks.Task<int> GetPreDesigByAsync(string EmployeeID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetGlobalEnum", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetGlobalEnum();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetGlobalEnum", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetGlobalEnumAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetDeptNameByDeptId", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetDeptNameByDeptId(int DeptId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetDeptNameByDeptId", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetDeptNameByDeptIdAsync(int DeptId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetGlobalDept", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetGlobalDept();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetGlobalDept", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetGlobalDeptAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetByUserID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetByUserID(string UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetByUserID", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetByUserIDAsync(string UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetByUserBr", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetByUserBr(string UserID, string branchCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetByUserBr", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetByUserBrAsync(string UserID, string branchCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetByUserIDCheck", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetByUserIDCheck(string UserID, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetByUserIDCheck", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetByUserIDCheckAsync(string UserID, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetGlobalAllBranch", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable GetGlobalAllBranch();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://onebank.com.bd/GetGlobalAllBranch", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetGlobalAllBranchAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface OBLAPPSoapChannel : CardChequeModule.ServiceReference.OBLAPPSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OBLAPPSoapClient : System.ServiceModel.ClientBase<CardChequeModule.ServiceReference.OBLAPPSoap>, CardChequeModule.ServiceReference.OBLAPPSoap {
        
        public OBLAPPSoapClient() {
        }
        
        public OBLAPPSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OBLAPPSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OBLAPPSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OBLAPPSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string HelloWorld() {
            return base.Channel.HelloWorld();
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync() {
            return base.Channel.HelloWorldAsync();
        }
        
        public System.Data.DataTable GetAllUser() {
            return base.Channel.GetAllUser();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetAllUserAsync() {
            return base.Channel.GetAllUserAsync();
        }
        
        public string GetEmployeeNameBy(string EmployeeID) {
            return base.Channel.GetEmployeeNameBy(EmployeeID);
        }
        
        public System.Threading.Tasks.Task<string> GetEmployeeNameByAsync(string EmployeeID) {
            return base.Channel.GetEmployeeNameByAsync(EmployeeID);
        }
        
        public long GetIDbyEmployeeId(string EmployeeID) {
            return base.Channel.GetIDbyEmployeeId(EmployeeID);
        }
        
        public System.Threading.Tasks.Task<long> GetIDbyEmployeeIdAsync(string EmployeeID) {
            return base.Channel.GetIDbyEmployeeIdAsync(EmployeeID);
        }
        
        public string GetPreDeptNameBy(string EmployeeID) {
            return base.Channel.GetPreDeptNameBy(EmployeeID);
        }
        
        public System.Threading.Tasks.Task<string> GetPreDeptNameByAsync(string EmployeeID) {
            return base.Channel.GetPreDeptNameByAsync(EmployeeID);
        }
        
        public int GetPreDeptIdBy(string EmployeeID) {
            return base.Channel.GetPreDeptIdBy(EmployeeID);
        }
        
        public System.Threading.Tasks.Task<int> GetPreDeptIdByAsync(string EmployeeID) {
            return base.Channel.GetPreDeptIdByAsync(EmployeeID);
        }
        
        public int GetPreBranchIdBy(string EmployeeID) {
            return base.Channel.GetPreBranchIdBy(EmployeeID);
        }
        
        public System.Threading.Tasks.Task<int> GetPreBranchIdByAsync(string EmployeeID) {
            return base.Channel.GetPreBranchIdByAsync(EmployeeID);
        }
        
        public int GetPreDesigBy(string EmployeeID) {
            return base.Channel.GetPreDesigBy(EmployeeID);
        }
        
        public System.Threading.Tasks.Task<int> GetPreDesigByAsync(string EmployeeID) {
            return base.Channel.GetPreDesigByAsync(EmployeeID);
        }
        
        public System.Data.DataTable GetGlobalEnum() {
            return base.Channel.GetGlobalEnum();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetGlobalEnumAsync() {
            return base.Channel.GetGlobalEnumAsync();
        }
        
        public string GetDeptNameByDeptId(int DeptId) {
            return base.Channel.GetDeptNameByDeptId(DeptId);
        }
        
        public System.Threading.Tasks.Task<string> GetDeptNameByDeptIdAsync(int DeptId) {
            return base.Channel.GetDeptNameByDeptIdAsync(DeptId);
        }
        
        public System.Data.DataTable GetGlobalDept() {
            return base.Channel.GetGlobalDept();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetGlobalDeptAsync() {
            return base.Channel.GetGlobalDeptAsync();
        }
        
        public System.Data.DataTable GetByUserID(string UserID) {
            return base.Channel.GetByUserID(UserID);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetByUserIDAsync(string UserID) {
            return base.Channel.GetByUserIDAsync(UserID);
        }
        
        public System.Data.DataTable GetByUserBr(string UserID, string branchCode) {
            return base.Channel.GetByUserBr(UserID, branchCode);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetByUserBrAsync(string UserID, string branchCode) {
            return base.Channel.GetByUserBrAsync(UserID, branchCode);
        }
        
        public string GetByUserIDCheck(string UserID, string password) {
            return base.Channel.GetByUserIDCheck(UserID, password);
        }
        
        public System.Threading.Tasks.Task<string> GetByUserIDCheckAsync(string UserID, string password) {
            return base.Channel.GetByUserIDCheckAsync(UserID, password);
        }
        
        public System.Data.DataTable GetGlobalAllBranch() {
            return base.Channel.GetGlobalAllBranch();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetGlobalAllBranchAsync() {
            return base.Channel.GetGlobalAllBranchAsync();
        }
    }
}
