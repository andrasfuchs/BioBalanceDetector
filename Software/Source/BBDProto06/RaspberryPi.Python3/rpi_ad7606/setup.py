# -*- coding: utf-8 -*-
"""
Alpha library for the AD7606-6 for use with a Raspberry Pi
Written by Benjamin Sumlin, Washington University in St. Louis
Aerosol Impacts and Research Laboratory
Department of Energy, Environmental, and Chemical Engineering
"""
from setuptools import setup
import re
VERSIONFILE="rpi_ad7606/_version.py"
verstrline = open(VERSIONFILE, "rt").read()
VSRE = r"^__version__ = ['\"]([^'\"]*)['\"]"
mo = re.search(VSRE, verstrline, re.M)
if mo:
  verstr = mo.group(1)
else:
  raise RuntimeError("Unable to find version string in %s." % (VERSIONFILE,))

setup(name='rpi_ad7606',
      version=verstr,
      description="Alpha library for interfacing a Raspberry Pi with an Analog Devices AD7606 ADC.",
      long_description=verstr + ' - SUPER ALPHA.',
      url='http://air.eece.wustl.edu/people/ben-sumlin/',
      author='Benjamin Sumlin',
      author_email='bsumlin@wustl.edu',
      license='MIT',
      packages=['rpi_ad7606'],
      keywords=['ADC'],
      classifiers = ['Development Status :: 3 - Alpha','Intended Audience :: Science/Research','Programming Language :: Python :: 3 :: Only'],
      install_requires=['pigpio','numpy'],
      python_requires='>=3',
      zip_safe=False)
