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

package com.astock;

public class CompanyInfo extends com.zeroc.Ice.Value
{
    public CompanyInfo()
    {
        this.name = "";
        this.addr = "";
    }

    public CompanyInfo(int id, String name, String addr)
    {
        this.id = id;
        this.name = name;
        this.addr = addr;
    }

    public int id;

    public String name;

    public String addr;

    public CompanyInfo clone()
    {
        return (CompanyInfo)super.clone();
    }

    public static String ice_staticId()
    {
        return "::com::astock::CompanyInfo";
    }

    @Override
    public String ice_id()
    {
        return ice_staticId();
    }

    /** @hidden */
    public static final long serialVersionUID = -290028570L;

    /** @hidden */
    @Override
    protected void _iceWriteImpl(com.zeroc.Ice.OutputStream ostr_)
    {
        ostr_.startSlice(ice_staticId(), -1, true);
        ostr_.writeInt(id);
        ostr_.writeString(name);
        ostr_.writeString(addr);
        ostr_.endSlice();
    }

    /** @hidden */
    @Override
    protected void _iceReadImpl(com.zeroc.Ice.InputStream istr_)
    {
        istr_.startSlice();
        id = istr_.readInt();
        name = istr_.readString();
        addr = istr_.readString();
        istr_.endSlice();
    }
}