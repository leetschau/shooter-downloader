// ShooterContextMenuExt.h : Declaration of the CShooterContextMenuExt

#pragma once
#include "resource.h"       // main symbols

#include "ShooterExt_i.h"

#include <string>
#include <list>
typedef std::list< std::basic_string<TCHAR> > StringList;


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
	STDMETHODIMP GetCommandString(UINT_PTR, UINT, UINT*, LPSTR, UINT);
	STDMETHODIMP InvokeCommand(LPCMINVOKECOMMANDINFO);
	STDMETHODIMP QueryContextMenu(HMENU, UINT, UINT, UINT, UINT);

private:
	StringList m_fileList;

};

const static TCHAR SHOOTER_DLDR_FILE_NAME[] = _T("ShooterDownloader.exe");

OBJECT_ENTRY_AUTO(__uuidof(ShooterContextMenuExt), CShooterContextMenuExt)
