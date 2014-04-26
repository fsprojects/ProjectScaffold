#!/bin/bash
#workaround to handle different path for fsi
export FSHARPI=`which fsharpi`
cat - > fsharpi <<"EOF"
#!/bin/bash
$FSHARPI $@
EOF
chmod +x fsharpi
fsharpi rename.fsx $@
rm fsharpi
