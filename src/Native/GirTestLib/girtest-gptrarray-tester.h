#pragma once

#include <glib-object.h>

G_BEGIN_DECLS

#define GIRTEST_TYPE_GPTRARRAY_ARRAY_TESTER girtest_gptrarray_array_tester_get_type()

G_DECLARE_FINAL_TYPE(GirTestGPtrArrayTester, girtest_gptrarray_array_tester, GIRTEST, GPTRARRAY_ARRAY_TESTER, GObject)

GPtrArray*
girtest_gptrarray_array_tester_create_array_transfer_full(int first, int second, gboolean set_free_func);

GPtrArray *
girtest_gptrarray_array_tester_create_array_transfer_container(int first, int second, gboolean set_free_func);

GPtrArray *
girtest_gptrarray_array_tester_create_array_transfer_none(int first, int second, gboolean set_free_func);

int
girtest_gptrarray_array_tester_get_elem_transfer_full(GPtrArray *arr, int n);

int
girtest_gptrarray_array_tester_get_elem_transfer_container(GPtrArray *arr, int n);

int
girtest_gptrarray_array_tester_get_elem_transfer_none(GPtrArray *arr, int n);

G_END_DECLS
