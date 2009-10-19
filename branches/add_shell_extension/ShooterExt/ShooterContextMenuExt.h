// ShooterContextMenuExt.h : Declaration of the CShooterContextMenuExt

#pragma once
#include "resource.h"       // main symbols

#include "ShooterExt_i.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif



// CShooterContextMenuExt

class ATL_NO_VTABLE CShooterContextMenuExt :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CShooterContextMenuExt, &CLSID_ShooterContextMenuExt>,
	public IShellExtInit,
	public IContextMenu
{
public:
	CShooterContextMenuExt()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_SHOOTERCONTEXTMENUEXT)

DECLARE_NOT_AGGREGATABLE(CShooterContextMenuExt)

BEGIN_COM_MAP(CShooterContextMenuExt)
	COM_INTERFACE_ENTRY(IShellExtInit)
	COM_INTERFACE_ENTRY(IContextMenu)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:
	//IShellExtInit
	STDMETHODIMP Initialize(LPCITEMIDLIST, LPDATAOBJECT, HKEY);

	// IContextMenu
	STDMETHODIMP GetCommandString(UINT, UINT, UINT*, LPSTR, UINT);
	STDMETHODIMP InvokeCommand(LPCMINVOKECOMMANDINFO);
	STDMETHODIMP QueryContextMenu(HMENU, UINT, UINT, UINT, UINT);

protected:
    TCHAR m_szFile [MAX_PATH];
};

OBJECT_ENTRY_AUTO(__uuidof(ShooterContextMenuExt), CShooterContextMenuExt)
