#include "girtest-gptrarray-tester.h"

/**
 * GirTestGPtrArrayTester:
 *
 * Contains functions for testing bindings with gptrarray array types.
 */

struct _GirTestGPtrArrayTester
{
    GObject parent_instance;
};

G_DEFINE_TYPE(GirTestGPtrArrayTester, girtest_gptrarray_array_tester, G_TYPE_OBJECT)

static void
girtest_gptrarray_array_tester_init(GirTestGPtrArrayTester *value)
{
}

static void
girtest_gptrarray_array_tester_class_init(GirTestGPtrArrayTesterClass *class)
{
}

/**
 * girtest_gptrarray_array_tester_return_gptrarray:
 * @first: The first number in the array to be returned.
 * @second: The second number in the array to be returned.
 *
 * Simple test to return a GPtrArray and ensure the memory is addressable.
 *
 * Returns: (element-type GObject) (transfer full): A GPtrArray containing the two arguments.
 */
GPtrArray *
girtest_gptrarray_array_tester_return_gptrarray(int first, int second)
{
	GPtrArray *arr = g_ptr_array_new();
	g_ptr_array_add(arr, GINT_TO_POINTER(first));
	g_ptr_array_add(arr, GINT_TO_POINTER(second));
	return arr;
}

/**
 * girtest_gptrarray_array_tester_return_string_array:
 *
 * Simple test to return a GPtrArray of strings and ensure the memory is addressable.
 *
 * Returns: (element-type utf8) (transfer full): A GPtrArray containing three string elements.
 */
GPtrArray *
girtest_gptrarray_array_tester_return_string_array()
{
  static GPtrArray *parray = NULL;
  static const gchar *values[] = { "0", "1", "2" };
  gint i;

  if (parray == NULL)
  {
    parray = g_ptr_array_new ();
    for (i = 0; i < 3; i++)
      g_ptr_array_add (parray, (gpointer) values[i]);
  }

  return parray;

}