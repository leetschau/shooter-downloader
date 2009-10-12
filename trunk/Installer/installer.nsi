;--------------------------------
; Use Modern UI

!include "MUI2.nsh"
!include "LogicLib.nsh"

;--------------------------------
; User definitions
!define PRODUCT_NAME "ShooterDownloader"
!define PRODUCT_NAME_CHT "射手網字幕下載工具"
!define PRODUCT_VERSION "1.0.1.2"
!define PRODUCT_VERSION_DISPLAY "1.0"
!define FILE_VERSION "1.0.1.2"

;--------------------------------
;Interface Settings

!define MUI_ABORTWARNING

;--------------------------------
;Language Selection Dialog Settings

;Remember the installer language
!define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
!define MUI_LANGDLL_REGISTRY_KEY "Software\${PRODUCT_NAME}" 
!define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
  
;--------------------------------
; Pages

!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

!insertmacro MUI_LANGUAGE "TradChinese"
!insertmacro MUI_LANGUAGE "English"

;Language strings
LangString InstName ${LANG_TRADCHINESE} "${PRODUCT_NAME_CHT} ${PRODUCT_VERSION_DISPLAY}"
LangString InstName ${LANG_ENGLISH} "${PRODUCT_NAME} ${PRODUCT_VERSION_DISPLAY}"
LangString Shortcut ${LANG_TRADCHINESE} "${PRODUCT_NAME_CHT}"
LangString Shortcut ${LANG_ENGLISH} "${PRODUCT_NAME}"
LangString NAME_SecMain ${LANG_TRADCHINESE} "主程式"
LangString NAME_SecMain ${LANG_ENGLISH} "Main Application"
LangString NAME_SecShortcut ${LANG_TRADCHINESE} "開始功能表捷徑"
LangString NAME_SecShortcut ${LANG_ENGLISH} "Start Menu Shortcuts"
LangString NAME_SecDskShortcut ${LANG_TRADCHINESE} "建立桌面捷徑。"
LangString NAME_SecDskShortcut ${LANG_English} "Desktop Shortcut"
LangString DESC_SecMain ${LANG_TRADCHINESE} "安裝主要程式檔案。"
LangString DESC_SecMain ${LANG_ENGLISH} "Main application files."
LangString DESC_SecShortcut ${LANG_TRADCHINESE} "在開始功能表中建立捷徑。"
LangString DESC_SecShortcut ${LANG_ENGLISH} "Create shortcuts in Start menu."
LangString DESC_SecDskShortcut ${LANG_TRADCHINESE} "在桌面建立捷徑。"
LangString DESC_SecDskShortcut ${LANG_English} "Create a shortcut on the desktop."
LangString WARN_DotNetNotInstalled ${LANG_TRADCHINESE} "未安裝 .Net Framework 2.0。若未安裝 .Net Framework 2.0 本程式將無法正常運作"
LangString WARN_DotNetNotInstalled ${LANG_ENGLISH} ".Net Framework 2.0 is not installed. This application won't work without .Net Framework 2.0."

; ------------------------------------------
; Installer Properties

; The name of the installer
Name $(InstName)

; The file to write
OutFile "ShooterDownloader_${FILE_VERSION}.exe"

; The default installation directory
InstallDir $PROGRAMFILES\${PRODUCT_NAME}

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\${PRODUCT_NAME}" "Install_Dir"

;Add XP manifest 
XPStyle on

; Request application privileges for Windows Vista
RequestExecutionLevel admin

VIProductVersion ${PRODUCT_VERSION}

VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" ${PRODUCT_NAME}
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductVersion" ${PRODUCT_VERSION}
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "John Fung"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" ${File_VERSION}
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" ${PRODUCT_NAME}

VIAddVersionKey /LANG=${LANG_TRADCHINESE} "ProductName" ${PRODUCT_NAME_CHT}
VIAddVersionKey /LANG=${LANG_TRADCHINESE} "ProductVersion" ${PRODUCT_VERSION}
VIAddVersionKey /LANG=${LANG_TRADCHINESE} "LegalCopyright" "John Fung"
VIAddVersionKey /LANG=${LANG_TRADCHINESE} "FileVersion" ${File_VERSION}
VIAddVersionKey /LANG=${LANG_TRADCHINESE} "FileDescription" ${PRODUCT_NAME_CHT}

;--------------------------------
;Reserve Files
  
;If you are using solid compression, files that are required before
;the actual installation should be stored first in the data block,
;because this will make your installer start faster.

!insertmacro MUI_RESERVEFILE_LANGDLL


;--------------------------------

; The stuff to install
Section "main" SecMain

	SectionIn RO

	; Set output path to the installation directory.
	SetOutPath $INSTDIR

	; Put file there
	File "..\ShooterDownloader\bin\Release\ShooterDownloader.exe"
	File "..\ShooterDownloader\bin\Release\ShooterDownloader.exe.config"
	File "..\ShooterDownloader\bin\Release\NCharDet.dll"

	; Write the installation path into the registry
	WriteRegStr HKLM "Software\${PRODUCT_NAME}" "Install_Dir" "$INSTDIR"

	; Write the uninstall keys for Windows
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayName" "${PRODUCT_NAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString" '"$INSTDIR\uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "NoRepair" 1
	WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "shortcut" SecShortcut

	CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
	CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
	CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\$(Shortcut).lnk" "$INSTDIR\ShooterDownloader.exe" "" "$INSTDIR\ShooterDownloader.exe" 0

SectionEnd

Section "dskShortcut" SecDskShortcut
	CreateShortCut "$DESKTOP\$(Shortcut).lnk" "$INSTDIR\ShooterDownloader.exe" "" "$INSTDIR\ShooterDownloader.exe" 0
SectionEnd


;--------------------------------
;Descriptions

;Assign language strings to sections
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${SecMain} $(DESC_SecMain)
!insertmacro MUI_DESCRIPTION_TEXT ${SecShortcut} $(DESC_SecShortcut)
!insertmacro MUI_DESCRIPTION_TEXT ${SecDskShortcut} $(DESC_SecDskShortcut)
!insertmacro MUI_FUNCTION_DESCRIPTION_END


;--------------------------------

; Uninstaller

Section "Uninstall" 
	; Remove registry keys
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
	DeleteRegKey HKLM "SOFTWARE\${PRODUCT_NAME}"

	; Remove files and uninstaller
	Delete $INSTDIR\ShooterDownloader.exe
	Delete $INSTDIR\ShooterDownloader.exe.config
	Delete $INSTDIR\NCharDet.dll
	Delete $INSTDIR\uninstall.exe

	; Remove shortcuts, if any
	Delete "$SMPROGRAMS\${PRODUCT_NAME}\*.*"
	Delete "$DESKTOP\$(Shortcut).lnk"

	; Remove directories used
	RMDir "$SMPROGRAMS\${PRODUCT_NAME}"
	RMDir "$INSTDIR"
SectionEnd

;--------------------------------
;Installer Functions

Function .onInit
	!insertmacro MUI_LANGDLL_DISPLAY


	SectionSetText ${SecMain} $(NAME_SecMain)
	SectionSetText ${SecShortcut} $(NAME_SecShortcut)
	SectionSetText ${SecDskShortcut} $(NAME_SecDskShortcut)
	
	Call IsNetfx20Installed
	Pop $R0
	
	${If} $R0 == "-1"
		MessageBox MB_OK|MB_ICONEXCLAMATION $(WARN_DotNetNotInstalled)
	${EndIf}
FunctionEnd

Function un.onInit
	!insertmacro MUI_UNGETLANGUAGE
FunctionEnd

Function IsNetfx20Installed
	;Check if Net 2.0 is install
	;Push 0 for true, Push -1 for false
	ReadRegStr $R0 HKLM "Software\Microsoft\NET Framework Setup\NDP\v2.0.50727" "SP"
	
	${If} $R0 == ""
		Push -1
	${Else}
		Push 0
	${EndIf}
FunctionEnd
