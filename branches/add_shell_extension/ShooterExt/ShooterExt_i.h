

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0500 */
/* at Tue Oct 20 17:41:11 2009
 */
/* Compiler settings for .\ShooterExt.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __ShooterExt_i_h__
#define __ShooterExt_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IShooterContextMenuExt_FWD_DEFINED__
#define __IShooterContextMenuExt_FWD_DEFINED__
typedef interface IShooterContextMenuExt IShooterContextMenuExt;
#endif 	/* __IShooterContextMenuExt_FWD_DEFINED__ */


#ifndef __ShooterContextMenuExt_FWD_DEFINED__
#define __ShooterContextMenuExt_FWD_DEFINED__

#ifdef __cplusplus
typedef class ShooterContextMenuExt ShooterContextMenuExt;
#else
typedef struct ShooterContextMenuExt ShooterContextMenuExt;
#endif /* __cplusplus */

#endif 	/* __ShooterContextMenuExt_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IShooterContextMenuExt_INTERFACE_DEFINED__
#define __IShooterContextMenuExt_INTERFACE_DEFINED__

/* interface IShooterContextMenuExt */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IShooterContextMenuExt;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("ECABC9F3-A74D-4ED5-9430-DE74237396FB")
    IShooterContextMenuExt : public IUnknown
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct IShooterContextMenuExtVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShooterContextMenuExt * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShooterContextMenuExt * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShooterContextMenuExt * This);
        
        END_INTERFACE
    } IShooterContextMenuExtVtbl;

    interface IShooterContextMenuExt
    {
        CONST_VTBL struct IShooterContextMenuExtVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShooterContextMenuExt_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShooterContextMenuExt_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShooterContextMenuExt_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShooterContextMenuExt_INTERFACE_DEFINED__ */



#ifndef __ShooterExtLib_LIBRARY_DEFINED__
#define __ShooterExtLib_LIBRARY_DEFINED__

/* library ShooterExtLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_ShooterExtLib;

EXTERN_C const CLSID CLSID_ShooterContextMenuExt;

#ifdef __cplusplus

class DECLSPEC_UUID("5258ACEF-6A10-4CDC-B5A5-B4B0D7EF23B2")
ShooterContextMenuExt;
#endif
#endif /* __ShooterExtLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


