#include "FileReader.h"

FString UFileReader::ReadFile() const
{
	FString AbsolutePath = FPaths::ProjectConfigDir();
	AbsolutePath.Append("Input/");
	AbsolutePath.Append("Day" + FString::Printf(TEXT("%02d"), Day) + ".txt");

	IPlatformFile& FileManager = FPlatformFileManager::Get().GetPlatformFile();
	FString FileContent;
	if (FileManager.FileExists(*AbsolutePath))
	{
		// We use the LoadFileToString to load the file into
		if (!FFileHelper::LoadFileToString(FileContent, *AbsolutePath, FFileHelper::EHashOptions::None))
		{
			UE_LOG(LogTemp, Error, TEXT("ReadFile: Did not load text from file %s"), *AbsolutePath);
		}
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("ReadFile: Expected file location: %s"), *AbsolutePath);
	}

	return FileContent;
}

TArray<FString> UFileReader::ReadLines() const
{
	TArray<FString> Lines;
	ReadFile().ParseIntoArrayLines(Lines);
	return Lines;
}
