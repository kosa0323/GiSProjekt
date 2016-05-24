// TestCpp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <cstdlib>

using namespace std;


int main()
{
	int *asd;
	int k = 12;
	asd = &k;

	cout << *asd << endl << asd << endl << &k;
	cin >> *asd;
    return 0;
}

