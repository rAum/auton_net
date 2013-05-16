#!/bin/bash
#
# Patch script for boost::shared_ptr.
# Adds an explicit copy constructor, beacuse in c++11 defining a move
# constructor without specifying a copy constructor the latter is marked as
# "deleted" and is not available.
#
# This prevents ROS from compiling in c++11 mode, so we want that constructor
# to be explicitly defined.
#
# If the script doesn't work, add the copy constructor by yourself (look at
# lines 52-56 in this file.

SHAREDPTR_HPP=`find /usr/include -name shared_ptr.hpp | grep boost/smart_ptr/shared_ptr.hpp`

MD5_CURR=`md5sum "${SHAREDPTR_HPP}" | cut -d\  -f 1`
MD5_ORIG='6a2adf82c58972296663050b4c6c3c84'
MD5_PATCHED='be9f368c4efe93b0e10cbb224dc7485d'

PATCH_FILE=/tmp/shared_ptr.patch

if [ -z "${SHAREDPTR_HPP}" ]; then
    echo "boost/smart_ptr/shared_ptr.hpp not found!"
    exit 1
else
    echo "Creating backup in ${SHAREDPTR_HPP}.bak"
    cp ${SHAREDPTR_HPP} ${SHAREDPTR_HPP}.bak
fi

echo -n "Patching ${SHAREDPTR_HPP}... "

if [ "${MD5_CURR}" == "${MD5_PATCHED}" ]; then
    echo "already patched!"
else
    if [ "${MD5_CURR}" != "${MD5_ORIG}" ]; then
        echo "invalid md5, aborting!"
        echo "${MD5_CURR} current"
        echo "${MD5_ORIG} expected"
        if [ `grep 'shared_ptr\s+const\+&' "${SHAREDPTR_HPP}" | wc -l` -gt 0 ]; then
            echo "...but ${SHAREDPTR_HPP} seems to have a valid copy constructor! :)"
        fi
    else

        cd `dirname ${SHAREDPTR_HPP}`
        cat <<EOF | patch > /dev/null
--- /usr/include/boost/smart_ptr/shared_ptr.hpp	2013-05-14 15:42:04.255560899 +0200
+++ /usr/include/boost/smart_ptr/shared_ptr.hpp	2013-05-14 17:00:44.763812059 +0200
@@ -344,6 +344,11 @@
 
 #if defined( BOOST_HAS_RVALUE_REFS )
 
+    // dextero: generated copy constructor is deleted
+    shared_ptr( shared_ptr const & r ): px( r.px ), pn( r.pn )
+    {
+    }
+
     shared_ptr( shared_ptr && r ): px( r.px ), pn() // never throws
     {
         pn.swap( r.pn );
EOF

        echo "done!"
        echo `md5sum ${SHAREDPTR_HPP}`
        echo "${MD5_PATCHED} expected"
    fi
fi
