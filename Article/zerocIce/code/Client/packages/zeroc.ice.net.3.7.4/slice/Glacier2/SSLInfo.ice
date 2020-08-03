//
// Copyright (c) ZeroC, Inc. All rights reserved.
//

#pragma once

[["cpp:dll-export:GLACIER2_API"]]
[["cpp:doxygen:include:Glacier2/Glacier2.h"]]
[["cpp:header-ext:h"]]
[["cpp:include:Glacier2/Config.h"]]

[["js:module:ice"]]

[["objc:dll-export:GLACIER2_API"]]
[["objc:header-dir:objc"]]

[["python:pkgdir:Glacier2"]]

#include <Ice/BuiltinSequences.ice>

#ifndef __SLICE2JAVA_COMPAT__
[["java:package:com.zeroc"]]
#endif

["objc:prefix:GLACIER2"]
module Glacier2
{

/**
 *
 * Information taken from an SSL connection used for permissions
 * verification.
 *
 * @see PermissionsVerifier
 *
 */
struct SSLInfo
{
    /** The remote host. */
    string remoteHost;

    /** The remote port. */
    int remotePort;

    /** The router's host. */
    string localHost;

    /** The router's port. */
    int localPort;

    /** The negotiated cipher suite. */
    string cipher;

    /** The certificate chain. */
    Ice::StringSeq certs;
}

}
