public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
public delegate void Callback<T, U, V, X>(T arg1, U arg2, V arg3, X arg4);
public delegate void Callback<T, U, V, X, Z>(T arg1, U arg2, V arg3, X arg4, Z arg5);
public delegate void CallbackRefArg<T>(ref T arg);
