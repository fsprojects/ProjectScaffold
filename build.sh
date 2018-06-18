#!/usr/bin/env bash

set -eu

cd "$(dirname "$0")"

clear

dotnet restore build.proj

[ ! -e build.fsx ] && dotnet fake run init.fsx
dotnet fake build "$@"
