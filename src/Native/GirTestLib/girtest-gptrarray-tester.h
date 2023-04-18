#pragma once

#include <glib-object.h>

G_BEGIN_DECLS

#define GIRTEST_TYPE_GPTRARRAY_ARRAY_TESTER girtest_gptrarray_array_tester_get_type()

G_DECLARE_FINAL_TYPE(GirTestGPtrArrayTester, girtest_gptrarray_array_tester, GIRTEST, GPTRARRAY_ARRAY_TESTER, GObject)

GPtrArray*
girtest_gptrarray_array_tester_return_gptrarray(int first, int second);

GPtrArray *
girtest_gptrarray_array_tester_return_string_array();

G_END_DECLS
