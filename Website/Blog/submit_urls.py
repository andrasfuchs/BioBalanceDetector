#!/usr/bin/env python3

# Takes a sitemap file [1] and submits each URL to the Wayback Machine [2].
#
# Usage: python3 submit_urls.py sitemap.xml
#
# The script will contact the Wayback Machine for each URL in turn and request
# that it be saved [3]. The script prints (to standard output) the HTTP status
# code received from the Wayback Machine for each URL. The output looks like
# this:
#
#     200 OK: https://myblog.com/2018/06/some-article
#     502 Bad Gateway: https://invalid.example/
#     200 OK: https://myblog.com/2018/07/another-article
#
# (I haven't looked too deeply into this, but my suspicion is that 502 Bad
# Gateway is used as a catchall and is the only error code you'll see.)
#
# If any URL submission results in a status other than 200 OK then the script
# will exit with status 1; otherwise it will exit with status 0.
#
# This script requires Python 3 to be installed. It has been tested with Python
# 3.6.4 but will probably work with earlier versions.
#
# [1]: https://www.sitemaps.org/protocol.html
# [2]: https://web.archive.org/
# [3]: https://blog.archive.org/2017/01/25/see-something-save-something/
#
# This script was written by Benjamin Esham (https://esham.io) and is released
# under the following terms:
#
# This is free and unencumbered software released into the public domain.
#
# Anyone is free to copy, modify, publish, use, compile, sell, or distribute
# this software, either in source code form or as a compiled binary, for any
# purpose, commercial or non-commercial, and by any means.
#
# In jurisdictions that recognize copyright laws, the author or authors of this
# software dedicate any and all copyright interest in the software to the
# public domain. We make this dedication for the benefit of the public at large
# and to the detriment of our heirs and successors. We intend this dedication
# to be an overt act of relinquishment in perpetuity of all present and future
# rights to this software under copyright law.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
# ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#
# For more information, please refer to <http://unlicense.org/>

from sys import argv, exit
from time import sleep
from urllib.error import HTTPError
from urllib.request import urlopen
import xml.etree.ElementTree as ET

ns = {"urlset": "http://www.sitemaps.org/schemas/sitemap/0.9"}

any_failed = False

locs = ET.parse(argv[1]).getroot().findall(".//urlset:loc", ns)
for loc in locs:
    for url in loc.itertext():
        request_url = "https://web.archive.org/save/{}".format(url)
        try:
            with urlopen(request_url) as response:
                print("200 OK: {}".format(url))
        except HTTPError as err:
            print("{} {}: {}".format(err.code, err.reason, url))
            any_failed = True
        finally:
            sleep(5)

if any_failed:
    exit(1)
else:
    exit(0)