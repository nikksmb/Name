// RedBlackTreeLibrary.cpp: определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include "RedBlackTreeLibrary.h"


RedBlackTreeLibrary::RedBlackTree* TelephoneTree;
RedBlackTreeLibrary::RedBlackTree* SurnameTree;
RedBlackTreeLibrary::RedBlackTree* StreetTree;
FILE* fileBase;
void AddText(HWND hEdit, char* text);
EXPORT void InitializeMemory()
{
	mutex = CreateMutex(NULL, FALSE, L"Mutex");
    if (GetLastError() == ERROR_ALREADY_EXISTS)
    {
        mutex = OpenMutex(MUTEX_ALL_ACCESS, FALSE, L"Mutex");
        WaitForSingleObject(mutex, INFINITE);
        file = CreateFile(L"telbase.txt", GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
        fileSize = GetFileSize(file, hiWordSize);
        CloseHandle(file);
        fileMapping = OpenFileMapping(FILE_MAP_READ, FALSE, L"Base");
    }
    else
    {
	    file = CreateFile(L"telbase.txt", GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
        fileSize = GetFileSize(file, hiWordSize);
	    fileMapping = CreateFileMapping(file, NULL, PAGE_READONLY, 0, 0, L"Base");
        CloseHandle(file);
    }
    if (fileSize % MAX_MAP_SIZE != 0)
    {
        nIterations = fileSize / MAX_MAP_SIZE + 1;
    }
    else
    {
        nIterations /= MAX_MAP_SIZE;
    }
	ReleaseMutex(mutex);
}
EXPORT void InitializeTrees()
{

    #pragma warning(disable: 4996)
	char infoString[255];
	void* a1 = address;
	fgets(infoString, 255, fileBase);

	memcpy(address, infoString, 255);

	char* telephone = new char[30];
	char* surname = new char[30];
	char* street = new char[30];

	char strings[8][30];
	char* temp = new char[30];
	temp = strtok(infoString, ";");
	int i = 0;
	while (temp != NULL)
	{
		strcpy(strings[i], temp);
		temp = strtok(NULL, ";");
		i++;
	}

	strcpy(telephone, strings[0]);
	strcpy(surname, strings[1]);
	strcpy(street, strings[i - 4]);
	TelephoneTree =  new RedBlackTreeLibrary::RedBlackTree(telephone, address);
	SurnameTree = new RedBlackTreeLibrary::RedBlackTree(surname, address);
	StreetTree = new RedBlackTreeLibrary::RedBlackTree(street, address);

	i = 1;
	while (!feof(fileBase))
	{

		fgets(infoString, 255, fileBase);
		void* a = (void*)((int)(address)+255 * i);
		memcpy(a, infoString, 255);
		temp = strtok(infoString, ";");
		int k = 0;
		while (temp != NULL)
		{
			strcpy(strings[k], temp);
			temp = strtok(NULL, ";");
			k++;
		}

		strcpy(telephone, strings[0]);
		strcpy(surname, strings[1]);
		if (k > 5)
			strcpy(street, strings[k - 4]);
		else
			strcpy(street, strings[2]);

		TelephoneTree->InsertData(telephone, (void*)((int)address + 255 * i));
		SurnameTree->InsertData(surname, (void*)((int)address + 255 * i));
		StreetTree->InsertData(street, (void*)((int)address + 255 * i));
	
		i++;
	}
	delete(telephone);
	delete(surname);
	delete(temp);
}

bool checkEqual(std::string* arrayFields, std::string* str)
{
	 #pragma warning(disable: 4996)
    for (int i = 0; i < 8; i++)
    {
		if (std::strcmp(str[i].c_str(),"\0"))
        {
			if (std::strcmp(arrayFields[i].c_str(),str[i].c_str()))
            {
                return false;
            }
        }
    }
    return true;
}

EXPORT void FindSurname(std::string* str, HWND hWnd)
{
	 #pragma warning(disable: 4996)
	char *filemem;
    int fieldNo = 0;
    DWORD offset = 0;
    std::string analyzedString = "";
    std::string *arrayFields = new std::string[8]; 
    //for (auto a : listRecord)
    //{
    //    delete [] a;
    //}
    //listRecord.clear();
    int mapSize;
    bool isEndOfRecords = false;
    for (int i = 0; i < nIterations; i++)
    {
        if (i != nIterations - 1)
        {
            mapSize = MAX_MAP_SIZE;
        }
        else
        {
            mapSize = fileSize - offset;
        }
        filemem = (char*) MapViewOfFile(fileMapping, FILE_MAP_READ, 0, offset, mapSize);
        if (!isEndOfRecords)
        {
        if (i == nIterations - 1)
        {
            mapSize++;
        }
        for (int j = 0; j < mapSize; j++)
        {
            if (filemem[j] != '\n' && filemem[j] != '\0')
            {
                if (filemem[j] != ';')
                {
                    analyzedString += filemem[j];               
                }
                else
                {
                    arrayFields[fieldNo] = analyzedString;
                    fieldNo++;
                    analyzedString = "";
                }
            }
            else
            {
                if (fieldNo == 0)
                    if (analyzedString.find("endrecords") == 0)
                    {
                        isEndOfRecords = true;
                        break;
                    }
                arrayFields[fieldNo] = analyzedString;
                analyzedString = "";
                fieldNo = 0;
                if (checkEqual(arrayFields, str))
                {
					std::string  *newArrayFields = new std::string[8];
                    for (int i = 0; i < 8; i++)
                    {
                        newArrayFields[i].replace(0, arrayFields[i].length(), arrayFields[i]);
                    }
					 #pragma warning(disable: 4996)
					char * text = new char[50];
					int index = 0;
					for (int i = 0; i<8; i++)
					{
						for (int k = 0; k < newArrayFields[i].size(); k++)
						{
							text[index] = newArrayFields[i][k];
							index++;
						}
						text[index] = ' ';
						index++;
					}
					for (int i = index; i<50; i++)
					{
						text[i] = '\0'; 
					}
					AddText(hWnd, text);
				
                    
                }
            }
        }
        }
        UnmapViewOfFile(filemem);
        offset += MAX_MAP_SIZE;
    }
    delete [] arrayFields;











	//char *filemem;
 //   int fieldNo = 0;
 //   DWORD offset = 0;
 //   std::string analyzedString = "";
 //   std::string *arrayFields = new std::string[8]; 
 //   int mapSize;
 //   for (int i = 0; i < nIterations; i++)
 //   {
 //       if (i != nIterations - 1)
 //       {
 //           mapSize = MAX_MAP_SIZE;
 //       }
 //       else
 //       {
 //           mapSize = fileSize - offset;
 //       }
 //       filemem = (char*) MapViewOfFile(fileMapping, FILE_MAP_READ, 0, offset, mapSize);
 //       if (i == nIterations - 1)
 //       {
 //           mapSize++;
 //       }
 //       for (int j = 0; j < mapSize; j++)
 //       {
 //           if (filemem[j] != '\n' && filemem[j] != '\0')
 //           {
 //               if (filemem[j] != ';')
 //               {
 //                   analyzedString += filemem[j];               
 //               }
 //               else
 //               {
 //                   arrayFields[fieldNo] = analyzedString;
 //                   fieldNo++;
 //                   analyzedString = "";
 //               }
 //           }
 //           else
 //           {
 //               arrayFields[fieldNo] = analyzedString;
 //               analyzedString = "";
 //               fieldNo = 0;
 //               if (checkEqual(arrayFields, str))
 //               {
 //                   std::string  *newArrayFields = new std::string[8];
 //                   for (int i = 0; i < 8; i++)
 //                   {
 //                       newArrayFields[i].replace(0, arrayFields[i].length(), arrayFields[i]);
 //                   }
 //            //       listRecord.push_back(newArrayFields);
	//				 #pragma warning(disable: 4996)
	//				char * text = new char[newArrayFields->size() + 1];
	//				std::copy(newArrayFields->begin(), newArrayFields->end(), text);
	//				text[newArrayFields->size()] = '\0'; 
	//				AddText(hWnd, text);
	//				delete[] text;
 //               }
 //           }
 //       }
 //       UnmapViewOfFile(filemem);
 //       offset += MAX_MAP_SIZE;
 //   }
}

EXPORT void FindAdress(std::string* str, HWND hWnd)
{
	//std::vector<void*> answer;
	//char* str = new char[255];
	//answer = *StreetTree->FindNode(street);
	//for (int i = 0; i<answer.size(); i++)
	//{
	//	memcpy(str, answer[i], 255);
	//	AddText(hWnd, str);
	//}
	//rewind(fileBase);
}
EXPORT void FindTelephone(std::string* str, HWND hWnd)
{
	//std::vector<void*> answer;
	//char* str = new char[255];
	//answer = *TelephoneTree->FindNode(telephone);
	//for (int i = 0; i<answer.size(); i++)
	//{
	//	memcpy(str, answer[i], 255);
	//	AddText(hWnd, str);
	//}
	//rewind(fileBase);
}


void AddText(HWND hEdit, char* text)
{
	SendMessageA(hEdit, EM_SETSEL, -1, -1);
	SendMessageA(hEdit, EM_REPLACESEL, FALSE, (LPARAM)text);
	SendMessageA(hEdit, EM_REPLACESEL, FALSE, (LPARAM)"\n");
}




namespace RedBlackTreeLibrary
{

	RedBlackTree::RedBlackTree(char key[30], void * address)
	{
	//	nullNode = new Node(NULL, NULL, NULL, BLACK, "\0", NULL);
	//	root = new Node(nullNode, nullNode, nullNode, BLACK, key, address);

	}

	void RedBlackTree::InsertData(char key[30], void * address)
	{
	//	InsertNode(root, key, address);
	}

	void RedBlackTree::InsertNode(Node * root, char key[30], void * address)
	{
		//if (CompareKeys(key, root->key) == -1)
		//{
		//	if (root->left == nullNode)
		//	{
		//		root->left = new Node(nullNode, nullNode, root, RED, key, address);
		//		if (root->color == RED)
		//		{
		//			FixTree(root->left);
		//		}
		//	}
		//	else
		//	{
		//		InsertNode(root->left, key, address);
		//	}
		//}
		//else
		//{
		//	if (CompareKeys(key, root->key) == 1)
		//	{
		//		if (root->right == nullNode)
		//		{
		//			root->right = new Node(nullNode, nullNode, root, RED, key, address);
		//			if (root->color == RED)
		//			{
		//				FixTree(root->right);
		//			}
		//		}
		//		else
		//		{
		//			InsertNode(root->right, key, address);
		//		}
		//	}
		//	else
		//	{
		//		//if equal
		//		root->addressList.push_back(address);
		//	}
		//}

	}

	std::vector<void *> * RedBlackTree::FindNode(char key[30])
	{
		/*Node * current = root;
		bool continueSearch = true;
		std::vector<void *> list;
		while (continueSearch)
		{
			switch (CompareKeys(key, current->key))
			{
			case -1:
				if (current->left != nullNode)
				{
					current = current->left;
				}
				else
				{
					continueSearch = false;
					return &list;
				}
				break;
			case 0:
				continueSearch = false;
				return &(current->addressList);
				break;
			case 1:
				if (current->right != nullNode)
				{
					current = current->right;
				}
				else
				{
					continueSearch = false;
					return &list;
				}
				break;
			default:
				continueSearch = false;
				return &list;
				break;
			}
		}
		return &list;*/
		return NULL;
	}



	RedBlackTree::~RedBlackTree()
	{
	}

	//private
	void RedBlackTree::FixTree(Node * node)
	{
		/*while (node->parent->color == RED)
		{
			if (node->parent == node->parent->parent->left)
			{
				Node * n = node->parent->parent->right;
				if (n->color == RED)
				{
					node->parent->color = BLACK;
					n->color = BLACK;
					node->parent->parent->color = RED;
					node = node->parent->parent;
				}
				else
				{
					if (node == node->parent->right)
					{
						node = node->parent;
						RotateLeft(node);
					}
					node->parent->color = BLACK;
					node->parent->parent->color = RED;
					RotateRight(node->parent->parent);
				}
			}
			else
			{
				Node * n = node->parent->parent->left;
				if (n->color == RED)
				{
					node->parent->color = BLACK;
					n->color = BLACK;
					node->parent->parent->color = RED;
					node = node->parent->parent;
				}
				else
				{
					if (node == node->parent->left)
					{
						node = node->parent;
						RotateRight(node);
					}
					node->parent->color = BLACK;
					node->parent->parent->color = RED;
					RotateLeft(node->parent->parent);
				}
			}
		}
		root->color = BLACK;*/
	}

	void RedBlackTree::RotateLeft(Node * node)
	{
	/*	Node * rightNode = node->right;
		node->right = rightNode->left;

		if (rightNode->left != nullNode)
		{
			rightNode->left->parent = node;
		}
		rightNode->parent = node->parent;
		if (node->parent == nullNode)
		{
			root = rightNode;
		}
		else if (node->parent->left == node)
		{
			node->parent->left = rightNode;
		}
		else
		{
			node->parent->right = rightNode;
		}
		rightNode->left = node;
		node->parent = rightNode;*/

	}

	void RedBlackTree::RotateRight(Node * node)
	{
	/*	Node * leftNode = node->left;
		node->left = leftNode->right;

		if (leftNode->right != nullNode)
		{
			leftNode->right->parent = node;
		}
		leftNode->parent = node->parent;
		if (node->parent == nullNode)
		{
			root = leftNode;
		}
		else if (node->parent->right == node)
		{
			node->parent->right = leftNode;
		}
		else
		{
			node->parent->left = leftNode;
		}

		leftNode->right = node;
		node->parent = leftNode;*/
	}
	int RedBlackTree::CompareKeys(char key1[30], char key2[30])
	{
		//-1    left, key1<key2
		//0     equal, key1=key2
		//1     right, key1>key2
		/*if (strcmp(key1, key2) < 0)
			return -1;
		else
		if (strcmp(key1, key2) > 0)
			return 1;
		else
			return 0;*/
		return 0;
	}


}