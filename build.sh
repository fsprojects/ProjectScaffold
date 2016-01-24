#!/usr/bin/env bash

set -eu
set -o pipefail

cd `dirname $0`

PAKET_BOOTSTRAPPER_EXE=.paket/paket.bootstrapper.exe
PAKET_EXE=.paket/paket.exe
FAKE_EXE=packages/build/FAKE/tools/FAKE.exe

FSIARGS=""
OS=${OS:-"unknown"}
if [[ "$OS" != "Windows_NT" ]]
then
  FSIARGS="--fsiargs -d:MONO"
fi

function run() {
  if [[ "$OS" != "Windows_NT" ]]
  then
    mono "$@"
  else
    "$@"
  fi
}

run $PAKET_BOOTSTRAPPER_EXE

if [[ "$OS" != "Windows_NT" ]] &&
       [ $(certmgr -list -c Trust | grep X.509 | wc -l) -le 1 ]
then
  echo "Your Mono installation has no trusted SSL root certificates set up."
  echo "This may result in the Paket bootstrapper failing to download Paket"
  echo "because Github's SSL certificate can't be verified. One way to fix"
  echo "this issue would be to download the list of SSL root certificates"
  echo "from the Mozilla project by running the following command:"
  echo ""
  echo "    mozroots --import --sync"
  echo ""
  echo "This will import over 100 SSL root certificates into your Mono"
  echo "certificate repository. Then try running this script again."
  exit 1
fi

run $PAKET_EXE restore

[ ! -e build.fsx ] && run $PAKET_EXE update
[ ! -e build.fsx ] && run $FAKE_EXE init.fsx
run $FAKE_EXE "$@" $FSIARGS build.fsx

