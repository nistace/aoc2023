// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "FileReader.generated.h"


UCLASS(ClassGroup=(Custom), meta=(BlueprintSpawnableComponent))
class UNREAL_API UFileReader final : public UActorComponent
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	int Day;

	UFUNCTION(BlueprintCallable)
	FString ReadFile() const;

	UFUNCTION(BlueprintCallable)
	TArray<FString> ReadLines() const;
};
