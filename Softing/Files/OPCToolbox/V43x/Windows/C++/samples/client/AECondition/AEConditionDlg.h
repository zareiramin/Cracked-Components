//-----------------------------------------------------------------------------
//                                                                            |
//                   Softing Industrial Automation GmbH                       |
//                        Richard-Reitzner-Allee 6                            |
//                           85540 Haar, Germany                              |
//                                                                            |
//                 This is a part of the Softing OPC Toolbox                  |
//       Copyright (c) 1998 - 2012 Softing Industrial Automation GmbH         |
//                           All Rights Reserved                              |
//                                                                            |
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//                          OPC TOOLBOX C++ - Samples                         |
//                                                                            |
//  Filename    : AEConditionDlg.h                                            |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Sample classes and declarations                             |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------

#include "ClientApplication.h"
#include "afxwin.h"
#include "afxcmn.h"

#include "ClientAeSubscription.h"
#include <ClientAeCondition.h>

using namespace SoftingOPCToolboxClient;

#if !defined(AFX_AECONDITIONDLG_H__A492A329_1113_11D4_9200_009027A26C86__INCLUDED_)
#define AFX_AECONDITIONDLG_H__A492A329_1113_11D4_9200_009027A26C86__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000



class MyAeSubscription : public AeSubscription
{

public:

	MyAeSubscription(AeSession* parentSession): AeSubscription(parentSession) {};
	tstring getStateToString(EnumConditionState state);
	void handleAeConditionsChanged(const std::vector<AeCondition*>& someAeConditions);

};
/////////////////////////////////////////////////////////////////////////////
// CAEConditionDlg dialog



class CAEConditionDlg : public CDialog
{

// Construction
public:
	CAEConditionDlg(CWnd* pParent = NULL);  // standard constructor
	~CAEConditionDlg();
// Dialog Data
	//{{AFX_DATA(CAEConditionDlg)
	enum { IDD = IDD_AECONDITION_DIALOG };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAEConditionDlg)
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;
	CImageList m_imgList;
	CListCtrl m_conditionStateList;

	// Generated message map functions
	//{{AFX_MSG(CAEConditionDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	Application* m_application;
	AeSession* m_session;
	MyAeSubscription* m_subscription;

public:

	static CListCtrl conditionList;
	CRichEditCtrl RichEditor;
	afx_msg void OnBnClickedAcknowledgeConditions();
	afx_msg void OnBnClickedEnableConditions();
	afx_msg void OnBnClickedDisableConditions();
	afx_msg void OnBnClickedButtonConditionState();

};


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_AEConditionDLG_H__A492A329_1113_11D4_9200_009027A26C86__INCLUDED_)
