#!/bin/sh

# Author: Marc Christensen (mchristensen@novell.com)
#         Michael Hutchinson (mhutchinson@novell.com)

MONO_FRAMEWORK_PATH=/Library/Frameworks/Mono.framework/Versions/Current
export DYLD_FALLBACK_LIBRARY_PATH=$MONO_FRAMEWORK_PATH/lib:/lib:/usr/lib

#prevent Macports from messing up mono and pkg-config
export PATH="$MONO_FRAMEWORK_PATH/bin:$PATH"

DIR=$(cd "$(dirname "$0")"; pwd)

# $0 should contain the full path from the root i.e. /Applications/<folder>.app/Contents/MacOS/<script>
EXE_PATH="$DIR/ffly.exe"

# Work around a bug in 'exec' in older versions of macosx
OSX_VERSION=$(uname -r | cut -f1 -d.)
if [ $OSX_VERSION -lt 9 ]; then  # If OSX version is 10.4
	MONO_EXEC="exec mono"
else
	MONO_EXEC="exec -a ffly mono"
fi

#mono version check

REQUIRED_MAJOR=2
REQUIRED_MINOR=4
APPNAME="Community Flight Finder"

VERSION_TITLE="Cannot launch $APPNAME"
VERSION_MSG="$APPNAME requires the Mono Framework version $REQUIRED_MAJOR.$REQUIRED_MINOR or later."
DOWNLOAD_URL="http://www.go-mono.com/mono-downloads/download.html"

MONO_VERSION="$(mono --version | grep 'Mono JIT compiler version ' |  cut -f5 -d\ )"
MONO_VERSION_MAJOR="$(echo $MONO_VERSION | cut -f1 -d.)"
MONO_VERSION_MINOR="$(echo $MONO_VERSION | cut -f2 -d.)"
if [ -z "$MONO_VERSION" ] \
	|| [ $MONO_VERSION_MAJOR -lt $REQUIRED_MAJOR ] \
	|| [ $MONO_VERSION_MAJOR -eq $REQUIRED_MAJOR -a $MONO_VERSION_MINOR -lt $REQUIRED_MINOR ] 
then
	osascript \
	-e "set question to display dialog \"$VERSION_MSG\" with title \"$VERSION_TITLE\" buttons {\"Cancel\", \"Download...\"} default button 2" \
	-e "if button returned of question is equal to \"Download...\" then open location \"$DOWNLOAD_URL\""
	echo "$VERSION_TITLE"
	echo "$VERSION_MSG"
	exit 1
fi

# NOTE: remove this for stable releases
if [ -z "$MD_NO_DEBUG" ]; then
	_MONO_OPTIONS=${MONO_OPTIONS:---debug}
else
	_MONO_OPTIONS=$MONO_OPTIONS
fi

$MONO_EXEC $_MONO_OPTIONS "$EXE_PATH" $* >> ~/Documents/ffly-log.txt

