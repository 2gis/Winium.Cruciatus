#include "stdafx.h"
#include "Winium.Cruciatus.TouchSimulator.h"

POINTER_TOUCH_INFO _contact;

bool TouchDown(int x, int y)
{
	InitializeTouchInjection(1, TOUCH_FEEDBACK_DEFAULT);
	memset(&_contact, 0, sizeof(POINTER_TOUCH_INFO));
	_contact.pointerInfo.pointerType = PT_TOUCH;
	_contact.pointerInfo.pointerId = 0;
	_contact.pointerInfo.ptPixelLocation.x = x;
	_contact.pointerInfo.ptPixelLocation.y = y;

	_contact.touchFlags = TOUCH_FLAG_NONE;
	_contact.touchMask = TOUCH_MASK_CONTACTAREA | TOUCH_MASK_ORIENTATION | TOUCH_MASK_PRESSURE;
	_contact.orientation = 90;
	_contact.pressure = 32000;

	_contact.rcContact.top = _contact.pointerInfo.ptPixelLocation.y - 2;
	_contact.rcContact.bottom = _contact.pointerInfo.ptPixelLocation.y + 2;
	_contact.rcContact.left = _contact.pointerInfo.ptPixelLocation.x - 2;
	_contact.rcContact.right = _contact.pointerInfo.ptPixelLocation.x + 2;

	_contact.pointerInfo.pointerFlags = POINTER_FLAG_DOWN | POINTER_FLAG_INRANGE | POINTER_FLAG_INCONTACT;
	return InjectTouchInput(1, &_contact) != 0;
}

bool TouchUp(int x, int y)
{
	_contact.pointerInfo.ptPixelLocation.x = x;
	_contact.pointerInfo.ptPixelLocation.y = y;

	_contact.pointerInfo.pointerFlags = POINTER_FLAG_UP;
	return InjectTouchInput(1, &_contact) != 0;
}

bool TouchUpdate(int x, int y)
{
	_contact.pointerInfo.ptPixelLocation.x = x;
	_contact.pointerInfo.ptPixelLocation.y = y;

	_contact.pointerInfo.pointerFlags = POINTER_FLAG_UPDATE | POINTER_FLAG_INRANGE | POINTER_FLAG_INCONTACT;

	return InjectTouchInput(1, &_contact) != 0;
}

bool Tap(int x, int y)
{
	if (!TouchDown(x, y))
	{
		return false;
	}

	return TouchUp(x, y);
}

bool DoubleTap(int x, int y)
{
	if (!Tap(x, y))
	{
		return false;
	}
	Sleep(200);
	return Tap(x, y);
}

bool LongTap(int x, int y, int duration)
{
	if (!TouchDown(x, y))
	{
		return false;
	}
	ULONGLONG start = GetTickCount64();
	while (GetTickCount64() < start + duration)
	{
		if (!TouchUpdate(x, y))
		{
			return false;
		}
		Sleep(16);
	}
	return TouchUp(x, y);
}