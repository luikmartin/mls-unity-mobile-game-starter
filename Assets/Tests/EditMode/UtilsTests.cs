using UnityEngine;
using NUnit.Framework;

public class UtilsTests
{
	private static SaveFile SAVE_FILE_DUMMY_OBJECT = new(highScore: 1);
	private static string SAVE_FILE_DUMMY_JSON = "{\n    \"highScore\": 1\n}";

	[SetUp]
	public void SetUp()
	{
		Debug.Log("SetUp() runs before each test");
	}

	[Test]
	public void GivenEnumString_WhenParsed_ThenValidEnumIsReturned()
	{
		var languageEnumEN = Utils.ParseEnum<Language>("EN");

		Assert.AreEqual(languageEnumEN, Language.EN);
	}

	[Test]
	public void GivenSaveFileObject_WhenConvertedToJson_ThenValidSaveFileJsonIsReturned()
	{
		var saveFileJson = Utils.ToJson(SAVE_FILE_DUMMY_OBJECT);

		Assert.AreEqual(saveFileJson, SAVE_FILE_DUMMY_JSON);
	}

	[Test]
	public void GivenSaveFileJson_WhenConvertedFromJson_ThenValidSaveFileObjectIsReturned()
	{
		var saveFile = Utils.FromJson<SaveFile>(SAVE_FILE_DUMMY_JSON);

		Assert.AreEqual(saveFile.highScore, SAVE_FILE_DUMMY_OBJECT.highScore);
	}

	[TearDown]
	public void TearDown()
	{
		Debug.Log("TearDown() runs after each test");
	}
}
