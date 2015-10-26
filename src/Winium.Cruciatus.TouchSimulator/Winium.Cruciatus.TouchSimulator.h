extern "C"
{
	__declspec(dllexport) bool TouchUp(int x, int y);
	__declspec(dllexport) bool TouchDown(int x, int y);
	__declspec(dllexport) bool TouchUpdate(int x, int y);
	__declspec(dllexport) bool Tap(int x, int y);
	__declspec(dllexport) bool DoubleTap(int x, int y);
	__declspec(dllexport) bool LongTap(int x, int y, int duraction);
}
