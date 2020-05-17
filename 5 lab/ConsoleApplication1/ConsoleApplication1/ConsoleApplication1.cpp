#include <iostream>
#include <conio.h>
#include <windows.h>
#include <tlhelp32.h>
#include <iomanip>
#include <string>
using namespace std;

#ifdef IS64BIT
DWORD finAddress = 0x7FF'FFFFFFFF;
#else
DWORD finAddress = 0x7FFFFFFF;
#endif

void Result();
MEMORY_BASIC_INFORMATION mbi;
string typeProtectR;
string typeProtectRW;
string typeProtectRX;
string typeProtectRWX;
void processList()
{
	HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	PROCESSENTRY32 pstruct;
	pstruct.dwSize = sizeof(pstruct);
	Process32First(hSnapshot, &pstruct);
	cout << setw(6) << "ID" << " | " << setw(20) << "Process name" << "\n";

	do
	{
		wcout << setw(6) << pstruct.th32ProcessID << " | " << setw(20) << pstruct.szExeFile << "\n";
	}
	while (Process32Next(hSnapshot, &pstruct));
}

void coutAddress(DWORD address)
{
	cout << hex << "0x" << setfill('0') << setw(8) << address << setfill(' ') << dec;
}


void ResultPageType()
{
	switch (mbi.Protect)
	{
	case PAGE_READONLY:
		typeProtectR = " \n READONLY ";
		break;
	case PAGE_READWRITE:
		typeProtectRW = " \n READ / WRITE ";
		break;
	case PAGE_EXECUTE_READ:
		typeProtectRX = " \n EXECUTE / READ ";
		break;
	case PAGE_EXECUTE_READWRITE:
		typeProtectRWX = " \n EXECUTE / READ / WRITE ";
		break;
	}
}

void getMapProcess(DWORD id)
{
	HANDLE hHandle = OpenProcess(PROCESS_QUERY_INFORMATION, FALSE, id);
	if (hHandle != 0)
	{
		for (DWORD bAddress = 0; bAddress < finAddress; bAddress += mbi.RegionSize)
		{
			while (bAddress - finAddress >= mbi.RegionSize)
			{
				VirtualQueryEx(hHandle, (void*)bAddress, &mbi, sizeof(mbi));
				ResultPageType();
				Result();
				break;
			}
		}
	}
	else
	{
		cout << " \n Нет доступа!" << endl;
	}

}

void Result()
{
	cout << "Права по данному запросу:" << typeProtectRW << endl;
}

int main()
{
	setlocale(LC_ALL, "Russian");
	int id;
	processList();
	cout << "Введите ID:";
	cin >> id;
	if (id)
	{
		getMapProcess(id);
	}
	system("pause");
}
