#-----------------------------------------------------------------------------
#                                                                            |
#                   Softing Industrial Automation GmbH                       |
#                        Richard-Reitzner-Allee 6                            |
#                           85540 Haar, Germany                              |
#                                                                            |
#                 This is a part of the Softing OPC Toolbox                  |
#       Copyright (c) 1998 - 2011 Softing Industrial Automation GmbH         |
#                           All Rights Reserved                              |
#                                                                            |
#-----------------------------------------------------------------------------
#-----------------------------------------------------------------------------
#                             OPC Toolbox C++                                |
#                                                                            |
#  Filename    : OPCToolboxC++_gcc.mak                                       |
#  Version     : 4.30                                                        |
#  Date        : 01-August-2011                                              |
#                                                                            |
#  Description :   Project makefile for gcc/linux                            |
#                                                                            |
#-----------------------------------------------------------------------------
#
#
# Please use the following syntax:
#
#     make -f[filename] all <and parameters>
#
# for example for a Debian 2.6 Kernel, g++ 4.1.2:
#
# for debug:
# - compile
#     make -f[filename] all OS=Debian TARGET=x86 KERNEL=2.6 COMPILER=GCC4 DEBUG=1
# - clean
#     make -f[filename] clean OS=Debian TARGET=x86 KERNEL=2.6 COMPILER=GCC4 DEBUG=1
#
# for release:
# - compile
#     make -f[filename] all OS=Debian TARGET=x86 KERNEL=2.6 COMPILER=GCC4
# - clean
#     make -f[filename] clean OS=Debian TARGET=x86 KERNEL=2.6 COMPILER=GCC4
#
#
# -----------------------------------------------------------------------------

AS      = as
LD      = ld
CC      = g++
AR      = ar
NM      = nm
STRIP   = strip

ifeq "$(OS)" ""
OS = Debian
endif

ifeq "$(TARGET)" ""
TARGET = x86
endif

ifeq "$(TARGET)" "sh3"
CC = sh3-linux-g++
endif

ifeq "$(KERNEL)" ""
KERNEL = 2.6
endif

ifeq "$(COMPILER)" ""
COMPILER = GCC4
endif

ROOTDIR = ..
SRCDIR = ..
CURRENTDIR = .
BINDIR = ../../bin/$(KERNEL)
CLIENTDIR  = client
CLIENTAEDIR  = $(CLIENTDIR)/Ae
CLIENTDADIR  = $(CLIENTDIR)/Da
SERVERDIR  = server
SERVERAEDIR  = $(SERVERDIR)/Ae
SERVERDADIR  = $(SERVERDIR)/Da
OTBDIR = ../../core/include

CFLAGS  = -pthread -Wall -fno-strict-aliasing \
          -I $(SRCDIR) -I $(CLIENTDIR) -I $(CURRENTDIR) -I $(CLIENTAEDIR) -I $(CLIENTDADIR) -I $(SERVERDIR) -I $(SERVERAEDIR) -I $(SERVERDADIR) -I $(OTBDIR) $(EXTCFLAGS)

ifeq "$(DEBUG)" "1"
CFLAGS  := -g -DDEBUG $(CFLAGS)
OUTDIR = bin/$(OS)/$(TARGET)/$(COMPILER)/$(KERNEL)/Debug
LIBRARYSO = $(BINDIR)/libTBCD.so
else
CFLAGS  := -Os -DNDEBUG $(CFLAGS)
OUTDIR = bin/$(OS)/$(TARGET)/$(COMPILER)/$(KERNEL)/Release
LIBRARYSO = $(BINDIR)/libTBC.so
endif

COMPILE = $(CC) $(CFLAGS) -o $@ -c $<

OBJECTS = $(OUTDIR)/ValueQT.o \
	$(OUTDIR)/Trace.o \
	$(OUTDIR)/Mutex.o \
	$(OUTDIR)/PosixEvents.o \
	$(OUTDIR)/ClientAddressSpaceElementBrowseOptions.o \
	$(OUTDIR)/ClientAddressSpaceElement.o \
	$(OUTDIR)/ClientApplication.o \
	$(OUTDIR)/ClientObjectSpaceElement.o \
	$(OUTDIR)/ClientServerBrowser.o \
	$(OUTDIR)/ClientServerStatus.o \
	$(OUTDIR)/ClientAeCategory.o \
	$(OUTDIR)/ClientAeCondition.o \
	$(OUTDIR)/ClientAeSession.o \
	$(OUTDIR)/ClientAeSubscription.o \
	$(OUTDIR)/ClientDaAddressSpaceElement.o \
	$(OUTDIR)/ClientDaGetPropertiesOptions.o \
	$(OUTDIR)/ClientDaItem.o \
	$(OUTDIR)/ClientDaProperty.o \
	$(OUTDIR)/ClientDaSession.o \
	$(OUTDIR)/ClientDaSubscription.o \
	$(OUTDIR)/ServerAddressSpaceElement.o \
	$(OUTDIR)/ServerApplication.o \
	$(OUTDIR)/ServerCreator.o \
	$(OUTDIR)/ServerAeAddressSpaceElement.o \
	$(OUTDIR)/ServerAeAttribute.o \
	$(OUTDIR)/ServerAeCategory.o \
	$(OUTDIR)/ServerAeCondition.o \
	$(OUTDIR)/ServerAeEvent.o \
	$(OUTDIR)/ServerDaAddressSpaceElement.o \
	$(OUTDIR)/ServerDaProperty.o \
	$(OUTDIR)/ServerDaRequest.o \
	$(OUTDIR)/ServerDaSession.o \
	$(OUTDIR)/ServerDaTransaction.o


OBJDEPS = *.h OPCToolboxC++_gcc.mak

all: $(LIBRARYSO)

clean:
	-rm -f $(OBJECTS) $(LIBRARYSO)

$(OUTDIR)/ValueQT.o: ValueQT.cpp $(OBJDEPS)
	mkdir -p $(OUTDIR)
	mkdir -p $(BINDIR)
	$(COMPILE)

$(OUTDIR)/Trace.o: Trace.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/Mutex.o: Mutex.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/PosixEvents.o: PosixEvents.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientAddressSpaceElementBrowseOptions.o: $(CLIENTDIR)/ClientAddressSpaceElementBrowseOptions.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientAddressSpaceElement.o: $(CLIENTDIR)/ClientAddressSpaceElement.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientApplication.o: $(CLIENTDIR)/ClientApplication.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientObjectSpaceElement.o: $(CLIENTDIR)/ClientObjectSpaceElement.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientServerBrowser.o: $(CLIENTDIR)/ClientServerBrowser.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientServerStatus.o: $(CLIENTDIR)/ClientServerStatus.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientAeCategory.o: $(CLIENTAEDIR)/ClientAeCategory.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientAeCondition.o: $(CLIENTAEDIR)/ClientAeCondition.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientAeSession.o: $(CLIENTAEDIR)/ClientAeSession.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientAeSubscription.o: $(CLIENTAEDIR)/ClientAeSubscription.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientDaAddressSpaceElement.o: $(CLIENTDADIR)/ClientDaAddressSpaceElement.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientDaGetPropertiesOptions.o: $(CLIENTDADIR)/ClientDaGetPropertiesOptions.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientDaItem.o: $(CLIENTDADIR)/ClientDaItem.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientDaProperty.o: $(CLIENTDADIR)/ClientDaProperty.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientDaSession.o: $(CLIENTDADIR)/ClientDaSession.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ClientDaSubscription.o: $(CLIENTDADIR)/ClientDaSubscription.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerAddressSpaceElement.o: $(SERVERDIR)/ServerAddressSpaceElement.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerApplication.o: $(SERVERDIR)/ServerApplication.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerCreator.o: $(SERVERDIR)/ServerCreator.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerAeAddressSpaceElement.o: $(SERVERAEDIR)/ServerAeAddressSpaceElement.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerAeAttribute.o: $(SERVERAEDIR)/ServerAeAttribute.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerAeCategory.o: $(SERVERAEDIR)/ServerAeCategory.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerAeCondition.o: $(SERVERAEDIR)/ServerAeCondition.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerAeEvent.o: $(SERVERAEDIR)/ServerAeEvent.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerDaAddressSpaceElement.o: $(SERVERDADIR)/ServerDaAddressSpaceElement.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerDaProperty.o: $(SERVERDADIR)/ServerDaProperty.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerDaRequest.o: $(SERVERDADIR)/ServerDaRequest.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerDaSession.o: $(SERVERDADIR)/ServerDaSession.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/ServerDaTransaction.o: $(SERVERDADIR)/ServerDaTransaction.cpp $(OBJDEPS)
	$(COMPILE)


$(LIBRARYSO): $(OBJECTS)
	$(CC) -shared -g $(CFLAGS) -o $(LIBRARYSO) $(OBJECTS) -L$(BINDIR) -lOTB
