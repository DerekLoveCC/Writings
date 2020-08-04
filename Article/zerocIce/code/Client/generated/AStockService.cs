//
// Copyright (c) ZeroC, Inc. All rights reserved.
//
//
// Ice version 3.7.4
//
// <auto-generated>
//
// Generated from file `AStockService.ice'
//
// Warning: do not edit this file.
//
// </auto-generated>
//


using _System = global::System;

#pragma warning disable 1591

namespace com
{
    namespace astock
    {
        [global::System.Runtime.InteropServices.ComVisible(false)]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724")]
        [global::System.Serializable]
        public partial class CompanyInfo : global::Ice.Value
        {
            #region Slice data members

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            public int id;

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            public string name;

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            public string addr;

            #endregion

            partial void ice_initialize();

            #region Constructors

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            public CompanyInfo()
            {
                this.name = "";
                this.addr = "";
                ice_initialize();
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            public CompanyInfo(int id, string name, string addr)
            {
                this.id = id;
                this.name = name;
                this.addr = addr;
                ice_initialize();
            }

            #endregion

            private const string _id = "::com::astock::CompanyInfo";

            public static new string ice_staticId()
            {
                return _id;
            }
            public override string ice_id()
            {
                return _id;
            }

            #region Marshaling support

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            protected override void iceWriteImpl(global::Ice.OutputStream ostr_)
            {
                ostr_.startSlice(ice_staticId(), -1, true);
                ostr_.writeInt(id);
                ostr_.writeString(name);
                ostr_.writeString(addr);
                ostr_.endSlice();
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            protected override void iceReadImpl(global::Ice.InputStream istr_)
            {
                istr_.startSlice();
                id = istr_.readInt();
                name = istr_.readString();
                addr = istr_.readString();
                istr_.endSlice();
            }

            #endregion
        }
    }
}

namespace com
{
    namespace astock
    {
        [global::System.Runtime.InteropServices.ComVisible(false)]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1715")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722")]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724")]
        public partial interface AStockService : global::Ice.Object, AStockServiceOperations_
        {
        }
    }
}

namespace com
{
    namespace astock
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
        public delegate void Callback_AStockService_GetCompanyInfo(CompanyInfo ret);
    }
}

namespace com
{
    namespace astock
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
        public interface AStockServicePrx : global::Ice.ObjectPrx
        {
            CompanyInfo GetCompanyInfo(int id, global::Ice.OptionalContext context = new global::Ice.OptionalContext());

            global::System.Threading.Tasks.Task<CompanyInfo> GetCompanyInfoAsync(int id, global::Ice.OptionalContext context = new global::Ice.OptionalContext(), global::System.IProgress<bool> progress = null, global::System.Threading.CancellationToken cancel = new global::System.Threading.CancellationToken());

            global::Ice.AsyncResult<Callback_AStockService_GetCompanyInfo> begin_GetCompanyInfo(int id, global::Ice.OptionalContext context = new global::Ice.OptionalContext());

            global::Ice.AsyncResult begin_GetCompanyInfo(int id, global::Ice.AsyncCallback callback, object cookie);

            global::Ice.AsyncResult begin_GetCompanyInfo(int id, global::Ice.OptionalContext context, global::Ice.AsyncCallback callback, object cookie);

            CompanyInfo end_GetCompanyInfo(global::Ice.AsyncResult asyncResult);
        }
    }
}

namespace com
{
    namespace astock
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
        public interface AStockServiceOperations_
        {
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
            CompanyInfo GetCompanyInfo(int id, global::Ice.Current current = null);
        }
    }
}

namespace com
{
    namespace astock
    {
        [global::System.Runtime.InteropServices.ComVisible(false)]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
        [global::System.Serializable]
        public sealed class AStockServicePrxHelper : global::Ice.ObjectPrxHelperBase, AStockServicePrx
        {
            public AStockServicePrxHelper()
            {
            }

            public AStockServicePrxHelper(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : base(info, context)
            {
            }

            #region Synchronous operations

            public CompanyInfo GetCompanyInfo(int id, global::Ice.OptionalContext context = new global::Ice.OptionalContext())
            {
                try
                {
                    return _iceI_GetCompanyInfoAsync(id, context, null, global::System.Threading.CancellationToken.None, true).Result;
                }
                catch(global::System.AggregateException ex_)
                {
                    throw ex_.InnerException;
                }
            }

            #endregion

            #region Async Task operations

            public global::System.Threading.Tasks.Task<CompanyInfo> GetCompanyInfoAsync(int id, global::Ice.OptionalContext context = new global::Ice.OptionalContext(), global::System.IProgress<bool> progress = null, global::System.Threading.CancellationToken cancel = new global::System.Threading.CancellationToken())
            {
                return _iceI_GetCompanyInfoAsync(id, context, progress, cancel, false);
            }

            private global::System.Threading.Tasks.Task<CompanyInfo> _iceI_GetCompanyInfoAsync(int iceP_id, global::Ice.OptionalContext context, global::System.IProgress<bool> progress, global::System.Threading.CancellationToken cancel, bool synchronous)
            {
                iceCheckTwowayOnly(_GetCompanyInfo_name);
                var completed = new global::IceInternal.OperationTaskCompletionCallback<CompanyInfo>(progress, cancel);
                _iceI_GetCompanyInfo(iceP_id, context, synchronous, completed);
                return completed.Task;
            }

            private const string _GetCompanyInfo_name = "GetCompanyInfo";

            private void _iceI_GetCompanyInfo(int iceP_id, global::System.Collections.Generic.Dictionary<string, string> context, bool synchronous, global::IceInternal.OutgoingAsyncCompletionCallback completed)
            {
                var outAsync = getOutgoingAsync<CompanyInfo>(completed);
                outAsync.invoke(
                    _GetCompanyInfo_name,
                    global::Ice.OperationMode.Normal,
                    global::Ice.FormatType.DefaultFormat,
                    context,
                    synchronous,
                    write: (global::Ice.OutputStream ostr) =>
                    {
                        ostr.writeInt(iceP_id);
                    },
                    read: (global::Ice.InputStream istr) =>
                    {
                        CompanyInfo ret = null;
                        istr.readValue((CompanyInfo v) => {ret = v; });
                        istr.readPendingValues();
                        return ret;
                    });
            }

            #endregion

            #region Asynchronous operations

            public global::Ice.AsyncResult<Callback_AStockService_GetCompanyInfo> begin_GetCompanyInfo(int id, global::Ice.OptionalContext context = new global::Ice.OptionalContext())
            {
                return begin_GetCompanyInfo(id, context, null, null, false);
            }

            public global::Ice.AsyncResult begin_GetCompanyInfo(int id, global::Ice.AsyncCallback callback, object cookie)
            {
                return begin_GetCompanyInfo(id, new global::Ice.OptionalContext(), callback, cookie, false);
            }

            public global::Ice.AsyncResult begin_GetCompanyInfo(int id, global::Ice.OptionalContext context, global::Ice.AsyncCallback callback, object cookie)
            {
                return begin_GetCompanyInfo(id, context, callback, cookie, false);
            }

            public CompanyInfo end_GetCompanyInfo(global::Ice.AsyncResult asyncResult)
            {
                var resultI_ = global::IceInternal.AsyncResultI.check(asyncResult, this, _GetCompanyInfo_name);
                var outgoing_ = (global::IceInternal.OutgoingAsyncT<CompanyInfo>)resultI_.OutgoingAsync;
                return outgoing_.getResult(resultI_.wait());
            }

            private global::Ice.AsyncResult<Callback_AStockService_GetCompanyInfo> begin_GetCompanyInfo(int iceP_id, global::System.Collections.Generic.Dictionary<string, string> context, global::Ice.AsyncCallback completedCallback, object cookie, bool synchronous)
            {
                iceCheckAsyncTwowayOnly(_GetCompanyInfo_name);
                var completed = new global::IceInternal.OperationAsyncResultCompletionCallback<Callback_AStockService_GetCompanyInfo, CompanyInfo>(
                    (Callback_AStockService_GetCompanyInfo cb, CompanyInfo ret) =>
                    {
                        if(cb != null)
                        {
                            cb.Invoke(ret);
                        }
                    },
                    this, _GetCompanyInfo_name, cookie, completedCallback);
                _iceI_GetCompanyInfo(iceP_id, context, synchronous, completed);
                return completed;
            }

            #endregion

            #region Checked and unchecked cast operations

            public static AStockServicePrx checkedCast(global::Ice.ObjectPrx b)
            {
                if(b == null)
                {
                    return null;
                }
                AStockServicePrx r = b as AStockServicePrx;
                if((r == null) && b.ice_isA(ice_staticId()))
                {
                    AStockServicePrxHelper h = new AStockServicePrxHelper();
                    h.iceCopyFrom(b);
                    r = h;
                }
                return r;
            }

            public static AStockServicePrx checkedCast(global::Ice.ObjectPrx b, global::System.Collections.Generic.Dictionary<string, string> ctx)
            {
                if(b == null)
                {
                    return null;
                }
                AStockServicePrx r = b as AStockServicePrx;
                if((r == null) && b.ice_isA(ice_staticId(), ctx))
                {
                    AStockServicePrxHelper h = new AStockServicePrxHelper();
                    h.iceCopyFrom(b);
                    r = h;
                }
                return r;
            }

            public static AStockServicePrx checkedCast(global::Ice.ObjectPrx b, string f)
            {
                if(b == null)
                {
                    return null;
                }
                global::Ice.ObjectPrx bb = b.ice_facet(f);
                try
                {
                    if(bb.ice_isA(ice_staticId()))
                    {
                        AStockServicePrxHelper h = new AStockServicePrxHelper();
                        h.iceCopyFrom(bb);
                        return h;
                    }
                }
                catch(global::Ice.FacetNotExistException)
                {
                }
                return null;
            }

            public static AStockServicePrx checkedCast(global::Ice.ObjectPrx b, string f, global::System.Collections.Generic.Dictionary<string, string> ctx)
            {
                if(b == null)
                {
                    return null;
                }
                global::Ice.ObjectPrx bb = b.ice_facet(f);
                try
                {
                    if(bb.ice_isA(ice_staticId(), ctx))
                    {
                        AStockServicePrxHelper h = new AStockServicePrxHelper();
                        h.iceCopyFrom(bb);
                        return h;
                    }
                }
                catch(global::Ice.FacetNotExistException)
                {
                }
                return null;
            }

            public static AStockServicePrx uncheckedCast(global::Ice.ObjectPrx b)
            {
                if(b == null)
                {
                    return null;
                }
                AStockServicePrx r = b as AStockServicePrx;
                if(r == null)
                {
                    AStockServicePrxHelper h = new AStockServicePrxHelper();
                    h.iceCopyFrom(b);
                    r = h;
                }
                return r;
            }

            public static AStockServicePrx uncheckedCast(global::Ice.ObjectPrx b, string f)
            {
                if(b == null)
                {
                    return null;
                }
                global::Ice.ObjectPrx bb = b.ice_facet(f);
                AStockServicePrxHelper h = new AStockServicePrxHelper();
                h.iceCopyFrom(bb);
                return h;
            }

            private static readonly string[] _ids =
            {
                "::Ice::Object",
                "::com::astock::AStockService"
            };

            public static string ice_staticId()
            {
                return _ids[1];
            }

            #endregion

            #region Marshaling support

            public static void write(global::Ice.OutputStream ostr, AStockServicePrx v)
            {
                ostr.writeProxy(v);
            }

            public static AStockServicePrx read(global::Ice.InputStream istr)
            {
                global::Ice.ObjectPrx proxy = istr.readProxy();
                if(proxy != null)
                {
                    AStockServicePrxHelper result = new AStockServicePrxHelper();
                    result.iceCopyFrom(proxy);
                    return result;
                }
                return null;
            }

            #endregion
        }
    }
}

namespace com
{
    namespace astock
    {
        [global::System.Runtime.InteropServices.ComVisible(false)]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("slice2cs", "3.7.4")]
        public abstract class AStockServiceDisp_ : global::Ice.ObjectImpl, AStockService
        {
            #region Slice operations

            public abstract CompanyInfo GetCompanyInfo(int id, global::Ice.Current current = null);

            #endregion

            #region Slice type-related members

            private static readonly string[] _ids =
            {
                "::Ice::Object",
                "::com::astock::AStockService"
            };

            public override bool ice_isA(string s, global::Ice.Current current = null)
            {
                return global::System.Array.BinarySearch(_ids, s, IceUtilInternal.StringUtil.OrdinalStringComparer) >= 0;
            }

            public override string[] ice_ids(global::Ice.Current current = null)
            {
                return _ids;
            }

            public override string ice_id(global::Ice.Current current = null)
            {
                return _ids[1];
            }

            public static new string ice_staticId()
            {
                return _ids[1];
            }

            #endregion

            #region Operation dispatch

            [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
            public static global::System.Threading.Tasks.Task<global::Ice.OutputStream>
            iceD_GetCompanyInfo(AStockService obj, global::IceInternal.Incoming inS, global::Ice.Current current)
            {
                global::Ice.ObjectImpl.iceCheckMode(global::Ice.OperationMode.Normal, current.mode);
                var istr = inS.startReadParams();
                int iceP_id;
                iceP_id = istr.readInt();
                inS.endReadParams();
                var ret = obj.GetCompanyInfo(iceP_id, current);
                var ostr = inS.startWriteParams();
                ostr.writeValue(ret);
                ostr.writePendingValues();
                inS.endWriteParams(ostr);
                return inS.setResult(ostr);
            }

            private static readonly string[] _all =
            {
                "GetCompanyInfo",
                "ice_id",
                "ice_ids",
                "ice_isA",
                "ice_ping"
            };

            public override global::System.Threading.Tasks.Task<global::Ice.OutputStream>
            iceDispatch(global::IceInternal.Incoming inS, global::Ice.Current current)
            {
                int pos = global::System.Array.BinarySearch(_all, current.operation, global::IceUtilInternal.StringUtil.OrdinalStringComparer);
                if(pos < 0)
                {
                    throw new global::Ice.OperationNotExistException(current.id, current.facet, current.operation);
                }

                switch(pos)
                {
                    case 0:
                    {
                        return iceD_GetCompanyInfo(this, inS, current);
                    }
                    case 1:
                    {
                        return global::Ice.ObjectImpl.iceD_ice_id(this, inS, current);
                    }
                    case 2:
                    {
                        return global::Ice.ObjectImpl.iceD_ice_ids(this, inS, current);
                    }
                    case 3:
                    {
                        return global::Ice.ObjectImpl.iceD_ice_isA(this, inS, current);
                    }
                    case 4:
                    {
                        return global::Ice.ObjectImpl.iceD_ice_ping(this, inS, current);
                    }
                }

                global::System.Diagnostics.Debug.Assert(false);
                throw new global::Ice.OperationNotExistException(current.id, current.facet, current.operation);
            }

            #endregion
        }
    }
}