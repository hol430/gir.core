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
 * girtest_gptrarray_array_tester_create_array_transfer_full:
 * @first: The first number in the array to be returned.
 * @second: The second number in the array to be returned.
 * @set_free_func: True to set the free func to g_free. False otherwise.
 *
 * Simple test to return a GPtrArray with transfer: full and ensure the memory is addressable.
 *
 * Returns: (element-type GObject) (transfer full): A GPtrArray containing the two arguments.
 */
GPtrArray *
girtest_gptrarray_array_tester_create_array_transfer_full(int first,
                                                          int second,
                                                          gboolean set_free_func)
{
  int *i0 = g_malloc(sizeof(int));
  int *i1 = g_malloc(sizeof(int));
  *i0 = first;
  *i1 = second;
	GPtrArray *arr;
  if (set_free_func)
    arr = g_ptr_array_new_with_free_func(g_free);
  else
    arr = g_ptr_array_new();
	g_ptr_array_add(arr, i0);
	g_ptr_array_add(arr, i1);
	return arr;
}

/**
 * girtest_gptrarray_array_tester_create_array_transfer_container:
 * @first: The first number in the array to be returned.
 * @second: The second number in the array to be returned.
 * @set_free_func: True to set the free func to g_free. False otherwise.
 *
 * Simple test to return a GPtrArray with transfer: full and ensure the memory is addressable.
 *
 * Returns: (element-type GObject) (transfer container): A GPtrArray containing the two arguments.
 */
GPtrArray *
girtest_gptrarray_array_tester_create_array_transfer_container(int first,
                                                               int second,
                                                               gboolean set_free_func)
{
  int *i0 = g_malloc(sizeof(int));
  int *i1 = g_malloc(sizeof(int));
  *i0 = first;
  *i1 = second;
	GPtrArray *arr;
  if (set_free_func)
    arr = g_ptr_array_new_with_free_func(g_free);
  else
    arr = g_ptr_array_new();
	g_ptr_array_add(arr, i0);
	g_ptr_array_add(arr, i1);
	return arr;
}

/**
 * girtest_gptrarray_array_tester_create_array_transfer_none:
 * @first: The first number in the array to be returned.
 * @second: The second number in the array to be returned.
 * @set_free_func: True to set the free func to g_free. False otherwise.
 *
 * Simple test to return a GPtrArray with transfer: full and ensure the memory is addressable.
 *
 * Returns: (element-type GObject) (transfer none): A GPtrArray containing the two arguments.
 */
GPtrArray *
girtest_gptrarray_array_tester_create_array_transfer_none(int first,
                                                          int second,
                                                          gboolean set_free_func)
{
  int *i0 = g_malloc(sizeof(int));
  int *i1 = g_malloc(sizeof(int));
  *i0 = first;
  *i1 = second;
	GPtrArray *arr;
  if (set_free_func)
    arr = g_ptr_array_new_with_free_func(g_free);
  else
    arr = g_ptr_array_new();
	g_ptr_array_add(arr, i0);
	g_ptr_array_add(arr, i1);
	return arr;
}

/**
 * girtest_gptrarray_array_tester_get_elem_transfer_full:
 * @arr: (element-type GObject) (transfer full): An array.
 * @n: Index of the element to be returned.
 *
 * Return the n-th element of the array. Ownership of the array and its elements
 * will be transferred, which will result in them all being freed.
 */
int
girtest_gptrarray_array_tester_get_elem_transfer_full(GPtrArray *arr,
                                                      int n)
{
  g_assert(n < arr->len);
  gpointer ptr = arr->pdata[n];
  int x = *(int *)ptr;
  g_ptr_array_free(arr, TRUE);
  return x;
}

/**
 * girtest_gptrarray_array_tester_get_elem_transfer_container:
 * @arr: (element-type GObject) (transfer container): An array.
 * @n: Index of the element to be returned.
 *
 * Return the n-th element of the array. Ownership of the container (but not its
 * elemens) will be transferred to native code.
 */
int
girtest_gptrarray_array_tester_get_elem_transfer_container(GPtrArray *arr,
                                                           int n)
{
  g_assert(n < arr->len);
  gpointer ptr = arr->pdata[n];
  int x = *(int *)ptr;
  g_ptr_array_free(arr, FALSE);
  return x;
}

/**
 * girtest_gptrarray_array_tester_get_elem_transfer_none:
 * @arr: (element-type GObject) (transfer none): An array.
 * @n: Index of the element to be returned.
 *
 * Return the n-th element of the array. Ownership of the array will not be
 * transferred, so the array will not be freed.
 */
int
girtest_gptrarray_array_tester_get_elem_transfer_none(GPtrArray *arr,
                                                      int n)
{
  g_assert(n < arr->len);
  gpointer ptr = arr->pdata[n];
  int x = *(int *)ptr;
  return x;
}
