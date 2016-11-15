#----------------------------------------------------------------
# Generated CMake target import file for configuration "Release".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "clFFT" for configuration "Release"
set_property(TARGET clFFT APPEND PROPERTY IMPORTED_CONFIGURATIONS RELEASE)
set_target_properties(clFFT PROPERTIES
  IMPORTED_IMPLIB_RELEASE "${_IMPORT_PREFIX}/lib64/import/clFFT.lib"
  IMPORTED_LINK_INTERFACE_LIBRARIES_RELEASE "C:/Program Files (x86)/AMD APP SDK/2.9-1/lib/x86_64/OpenCL.lib"
  IMPORTED_LOCATION_RELEASE "${_IMPORT_PREFIX}/bin/clFFT.dll"
  )

list(APPEND _IMPORT_CHECK_TARGETS clFFT )
list(APPEND _IMPORT_CHECK_FILES_FOR_clFFT "${_IMPORT_PREFIX}/lib64/import/clFFT.lib" "${_IMPORT_PREFIX}/bin/clFFT.dll" )

# Import target "StatTimer" for configuration "Release"
set_property(TARGET StatTimer APPEND PROPERTY IMPORTED_CONFIGURATIONS RELEASE)
set_target_properties(StatTimer PROPERTIES
  IMPORTED_IMPLIB_RELEASE "${_IMPORT_PREFIX}/lib64/import/StatTimer.lib"
  IMPORTED_LINK_INTERFACE_LIBRARIES_RELEASE "C:/Program Files (x86)/AMD APP SDK/2.9-1/lib/x86_64/OpenCL.lib"
  IMPORTED_LOCATION_RELEASE "${_IMPORT_PREFIX}/bin/StatTimer.dll"
  )

list(APPEND _IMPORT_CHECK_TARGETS StatTimer )
list(APPEND _IMPORT_CHECK_FILES_FOR_StatTimer "${_IMPORT_PREFIX}/lib64/import/StatTimer.lib" "${_IMPORT_PREFIX}/bin/StatTimer.dll" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
