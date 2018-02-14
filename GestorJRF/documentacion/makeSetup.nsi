; ;NSIS Modern User Interface

;--------------------------------
  ;Include Modern UI
  !include "MUI2.nsh"
  !include x64.nsh
  ;Include NsProcess for process manage
  !include "nsProcess.nsh"

;--------------------------------
;General

  ;Name and file
	Name 'GestorJRF'
	OutFile "GestorJRF_Setup.exe"

  ;Default installation folder
	InstallDir "$PROGRAMFILES\GestorJRF"
  
  ;Get installation folder from registry if available
	InstallDirRegKey HKCU "Software\GestorJRF" ""

  ;Request application privileges for Windows 8
	RequestExecutionLevel admin

	Caption "GestorJRF INSTALLER"
	
	!define MUI_ICON "logo.ico"
	!define MUI_HEADERIMAGE
	!define MUI_HEADERIMAGE_BITMAP  "logo.ico"
	!define MUI_HEADERIMAGE_RIGHT
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_LICENSE "Licencia.txt"
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "Spanish"

;--------------------------------
;Installer Sections

Section "Dummy Section" SecDummy

  SetOutPath "$INSTDIR"
 
  ;Store installation folder
  WriteRegStr HKCU "Software\GestorJRF" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"


SectionEnd


Section -Prerequisites

${If} ${RunningX64}

	ReadRegStr $1 HKLM "SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x64" "Installed"

	StrCmp $1 1 installed
	
	;not installed, so run the installer

	ExecWait '"vcredist_x64.exe" /q /norestart'

	installed:
	;we are done

${Else}

	ReadRegStr $1 HKLM "SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x86" "Installed"

	StrCmp $1 1 installed86
	
	;not installed86, so run the installer

	ExecWait '"vcredist_x86.exe" /q /norestart'

	installed86:
	;we are done
	
${EndIf}


SectionEnd
#NET Framework 4.5 overwrite files of 4.0.30319 and the folder still beeing 4.0.30319 
!define NETVersion "4.0.30319"
!define NETInstaller "dotNetFx45_Full_setup.exe"

Section "MS .NET Framework v${NETVersion}" SecFramework
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v${NETVersion}" NETFrameworkInstalled 0


  DetailPrint "Starting Microsoft .NET Framework v${NETVersion} Setup..."
  ExecWait "${NETInstaller}"
  Return
 
  NETFrameworkInstalled:
  DetailPrint "Microsoft .NET Framework is already installed!"
 
SectionEnd

# default section
Section

;Relative path
#hay que cambiarlos con el originial del ordenador donde se crea.
!define MyPath "C:\Users\jruiz\source\repos\GestorJRF\GestorJRF"
!define MyPath2 "C:\Users\jruiz\source\repos\GestorJRF\GestorJRF\GestorJRF\Iconos"
!define MyPath3 "C:\Users\Eric Mendillo\Documents\Workspace\Windows\documentacion"

# define the output path for this file
SetOutPath $INSTDIR
 
# define what to install and place it in the output path
File "${MyPath}\GestorJRF\bin\debug\*"
File /r "${MyPath}\GestorJRF\bin\debug\*"
File "${MyPath2}\logo.ico"

CreateShortCut "$DESKTOP\GestorJRF.lnk" "$INSTDIR\GestorJRF.exe" ""

ClearErrors
SetRegView 32

WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\GestorJRF"  "Path"  "$APPDATA\GestorJRF\"

SetRegView 64
WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\GestorJRF" \
                 "DisplayName" "GestorJRF"
WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\GestorJRF" \
                 "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\GestorJRF" \
                 "DisplayIcon" "$\"$INSTDIR\logo.ico$\""

SectionEnd

;-------------------------------
;Uninstaller Section

Section "Uninstall"

	${nsProcess::KillProcess} "GestorJRF.exe" $R0
	${nsProcess::KillProcess} "GestorJRF.vshost.exe" $R0
	
	;ADD YOUR OWN FILES HERE...
	Delete "$INSTDIR\*"
	Delete "$INSTDIR\Uninstall.exe"
	Delete "$DESKTOP\GestorJRF.lnk"
	RMDir /r "$INSTDIR"

	DeleteRegKey /ifempty HKCU "Software\GestorJRF"
	ClearErrors
		
	SetRegView 64
	DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\GestorJRF" 
	DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\GestorJRF"
	
SectionEnd