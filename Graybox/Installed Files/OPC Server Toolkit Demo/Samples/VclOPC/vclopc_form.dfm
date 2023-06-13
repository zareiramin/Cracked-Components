object Form1: TForm1
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'Graybox Software VCLOPC Sample'
  ClientHeight = 428
  ClientWidth = 491
  Color = clWindow
  Ctl3D = False
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object StaticText1: TStaticText
    Left = 8
    Top = 8
    Width = 79
    Height = 17
    Caption = 'Clients number:'
    TabOrder = 0
  end
  object StaticText2: TStaticText
    Left = 8
    Top = 31
    Width = 73
    Height = 17
    Caption = 'Locks number:'
    TabOrder = 1
  end
  object StaticText3: TStaticText
    Left = 8
    Top = 54
    Width = 58
    Height = 17
    Caption = 'Bandwidth:'
    TabOrder = 2
  end
  object StringGridTags: TStringGrid
    Left = 0
    Top = 98
    Width = 491
    Height = 210
    Align = alBottom
    ColCount = 4
    Ctl3D = False
    DefaultColWidth = 80
    DefaultRowHeight = 18
    FixedCols = 2
    RowCount = 11
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goEditing]
    ParentCtl3D = False
    TabOrder = 3
    OnSetEditText = StringGridTagsSetEditText
    ColWidths = (
      80
      80
      155
      170)
  end
  object ListBoxLog: TListBox
    Left = 0
    Top = 308
    Width = 491
    Height = 120
    Align = alBottom
    Ctl3D = False
    ItemHeight = 13
    ParentCtl3D = False
    TabOrder = 4
  end
  object StaticTextClients: TStaticText
    Left = 112
    Top = 8
    Width = 10
    Height = 17
    Caption = '0'
    TabOrder = 5
  end
  object StaticTextLocks: TStaticText
    Left = 112
    Top = 31
    Width = 10
    Height = 17
    Caption = '0'
    TabOrder = 6
  end
  object StaticTextBand: TStaticText
    Left = 112
    Top = 54
    Width = 10
    Height = 17
    Caption = '0'
    TabOrder = 7
  end
  object ButtonShutdown: TButton
    Left = 408
    Top = 8
    Width = 75
    Height = 25
    Caption = 'Shutdown'
    TabOrder = 8
    OnClick = ButtonShutdownClick
  end
  object ApplicationEvents1: TApplicationEvents
    OnIdle = ApplicationEvents1Idle
    Left = 40
    Top = 352
  end
end
