#-----------------------------------------------------------------------------
#                                                                            |
#                               Softing AG                                   |
#                        Richard-Reitzner-Allee 6                            |
#                           85540 Haar, Germany                              |
#                                                                            |
#                 This is a part of the Softing OPC Toolbox                  |
#                   Copyright (C) Softing AG 1998 - 2008                     |
#                           All Rights Reserved                              |
#                                                                            |
#-----------------------------------------------------------------------------
#
# Project makefile for gcc/linux
#
# -----------------------------------------------------------------------------
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

ROOTDIR = .
LIBDIR  = $(ROOTDIR)
DEVBIN  = ../../../../bin/$(KERNEL)

CFLAGS  = -pthread -Wall -I ../../../../core/include -I ../../../src -I ../../../src/client -I ../../../src/client/Ae -I ../../../src/client/Da

ifeq "$(DEBUG)" "1"
CFLAGS  := -g -DDEBUG $(CFLAGS)
LIBOTB = OTB
LIBTBC = TBCD
OUTDIR = bin/$(OS)/$(TARGET)/$(COMPILER)/$(KERNEL)/Debug
BIN = ObjectAttributesD
else
CFLAGS  := -Os -DNDEBUG $(CFLAGS)
LIBOTB = OTB
LIBTBC = TBC
OUTDIR = bin/$(OS)/$(TARGET)/$(COMPILER)/$(KERNEL)/Release
BIN = ObjectAttributes
endif

COMPILE = $(CC) $(CFLAGS) -o $@ -c $<

OBJECTS = $(OUTDIR)/Console.o \
	$(OUTDIR)/OpcClient.o \
	$(OUTDIR)/stdafx.o

OBJDEPS = *.h ObjectAttributes_gcc.mak

all: $(OUTDIR)/$(BIN)

$(OUTDIR)/Console.o: Console.cpp $(OBJDEPS)
	mkdir -p $(OUTDIR)
	$(COMPILE)

$(OUTDIR)/OpcClient.o: OpcClient.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/stdafx.o: stdafx.cpp $(OBJDEPS)
	$(COMPILE)

$(OUTDIR)/$(BIN): $(OBJECTS)
	$(CC) -o $@ $(CFLAGS) $^ -L$(DEVBIN) -l$(LIBTBC) -l$(LIBOTB)

clean:
	-rm -f $(OBJECTS)
	-rm -f $(OUTDIR)/$(BIN)


