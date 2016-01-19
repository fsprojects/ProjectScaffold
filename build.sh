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
       [ ! -e ~/.config/.mono/certs ]
then
  mozroots --import --sync --quiet
fi

run $PAKET_EXE restore

[ ! -e build.fsx ] && run $PAKET_EXE update
[ ! -e build.fsx ] && run $FAKE_EXE init.fsx
run $FAKE_EXE "$@" $FSIARGS build.fsx

